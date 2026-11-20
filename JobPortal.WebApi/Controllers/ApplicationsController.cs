using JobPortal.Application.Features.JobApplications.Commands.ApplyJob;
using JobPortal.Application.Features.JobApplications.Commands.UpdateApplicationStatus;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationById;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJob;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJobSeeker;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByRecruiter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetApplicationByIdQuery
            {
                Id = id
            });

            if (result == null)
                return NotFound(new { Success = false, Message = $"Application with ID '{id}' was not found." });

            return Ok(result);
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJob(Guid jobId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            Guid? recruiterId = null;
            if (!User.IsInRole("Admin") && Guid.TryParse(userIdClaim, out var parsedRecruiterId))
            {
                recruiterId = parsedRecruiterId;
            }

            try
            {
                var result = await _mediator.Send(new GetApplicationsByJobQuery
                {
                    JobId = jobId,
                    RecruiterId = recruiterId
                });

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpGet("recruiter-applications")]
        [HttpGet("all-applicants")]
        public async Task<IActionResult> GetRecruiterApplications([FromQuery] string? status = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var recruiterId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing recruiter token." });

            var result = await _mediator.Send(new GetApplicationsByRecruiterQuery
            {
                RecruiterId = recruiterId,
                Status = status
            });

            return Ok(new
            {
                Success = true,
                TotalCount = result.Count,
                Applications = result
            });
        }

        [Authorize(Roles = "JobSeeker,Admin")]
        [HttpGet("jobseeker/{jobSeekerId}")]
        public async Task<IActionResult> GetByJobSeeker(Guid jobSeekerId, [FromQuery] string? status = null)
        {
            return Ok(await _mediator.Send(new GetApplicationsByJobSeekerQuery
            {
                JobSeekerId = jobSeekerId,
                Status = status
            }));
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications([FromQuery] string? status = null)
        {
            var seekerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(seekerIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            return Ok(await _mediator.Send(new GetApplicationsByJobSeekerQuery
            {
                JobSeekerId = seekerId,
                Status = status
            }));
        }

        [Authorize(Roles = "Recruiter,Admin")]
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateApplicationStatusCommand command)
        {
            var success = await _mediator.Send(command);
            if (!success)
                return NotFound(new { Success = false, Message = "Application not found or update failed." });

            return Ok(new
            {
                Success = true,
                Message = "Application status updated successfully."
            });
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyJob([FromBody] ApplyJobCommand command)
        {
            var seekerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (Guid.TryParse(seekerIdClaim, out var seekerId) && command.JobSeekerId == Guid.Empty)
            {
                command.JobSeekerId = seekerId;
            }

            var applicationId = await _mediator.Send(command);

            return Ok(new
            {
                Success = true,
                Message = "Application submitted successfully.",
                ApplicationId = applicationId
            });
        }
    }
}
