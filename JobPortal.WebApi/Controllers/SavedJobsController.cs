using JobPortal.Application.DTOs.SavedJobs;
using JobPortal.Application.Features.SavedJobs.Commands.DeleteSavedJob;
using JobPortal.Application.Features.SavedJobs.Commands.SaveJob;
using JobPortal.Application.Features.SavedJobs.Queries.GetSavedJobs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SavedJobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SavedJobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid? GetJobSeekerId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (Guid.TryParse(claim, out var id))
                return id;

            return null;
        }

        // 1. GET: /api/SavedJobs
        [HttpGet]
        public async Task<IActionResult> GetSavedJobs()
        {
            var seekerId = GetJobSeekerId();
            if (seekerId == null)
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing job seeker token." });

            var result = await _mediator.Send(new GetSavedJobsQuery
            {
                JobSeekerProfileId = seekerId.Value
            });

            return Ok(new
            {
                Success = true,
                TotalCount = result.Count,
                Data = result
            });
        }

        // 2. POST: /api/SavedJobs
        [HttpPost]
        public async Task<IActionResult> SaveJob([FromBody] SaveJobRequestDto request)
        {
            var seekerId = GetJobSeekerId();
            if (seekerId == null)
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing job seeker token." });

            if (request == null || request.JobId == Guid.Empty)
                return BadRequest(new { Success = false, Message = "JobId is required." });

            try
            {
                var result = await _mediator.Send(new SaveJobCommand
                {
                    JobSeekerProfileId = seekerId.Value,
                    JobId = request.JobId
                });

                return Ok(new
                {
                    Success = true,
                    Message = "Job saved successfully.",
                    Data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
        }

        // 3. POST: /api/SavedJobs/{jobId}
        [HttpPost("{jobId}")]
        public async Task<IActionResult> SaveJobByRoute(Guid jobId)
        {
            return await SaveJob(new SaveJobRequestDto { JobId = jobId });
        }

        // 4. DELETE: /api/SavedJobs/{jobId}
        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteSavedJob(Guid jobId)
        {
            var seekerId = GetJobSeekerId();
            if (seekerId == null)
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing job seeker token." });

            var success = await _mediator.Send(new DeleteSavedJobCommand
            {
                JobSeekerProfileId = seekerId.Value,
                JobId = jobId
            });

            if (!success)
            {
                return NotFound(new { Success = false, Message = "Saved job not found or already removed." });
            }

            return Ok(new
            {
                Success = true,
                Message = "Job removed from saved jobs successfully."
            });
        }
    }
}
