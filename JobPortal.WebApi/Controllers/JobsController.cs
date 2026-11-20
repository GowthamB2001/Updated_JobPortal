using JobPortal.Application.Features.Jobs.Commands.CreateJob;
using JobPortal.Application.Features.Jobs.Queries.GetJobById;
using JobPortal.Application.Features.Jobs.Commands.UpdateJob;
using JobPortal.Application.Features.Jobs.Commands.DeleteJob;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobPortal.Application.Features.Jobs.Queries.GetPagedJobs;
using JobPortal.Application.Features.Jobs.Queries.SearchJobs;
using JobPortal.Domain.Domain.Enums;
using JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter;
using System.Security.Claims;
using JobPortal.Application.Features.Jobs.Commands.RenewJob;
using JobPortal.Application.Features.Jobs.Queries.GetFilterPreferences;

namespace JobPortal.WebApi.Controllers
{
    [Authorize(Roles = "Recruiter")]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetJobByIdQuery
                {
                    Id = id
                });

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobCommand command)
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (Guid.TryParse(recruiterIdClaim, out var recruiterId) && command.RecruiterId == Guid.Empty)
            {
                command.RecruiterId = recruiterId;
            }

            var jobId = await _mediator.Send(command);

            return Ok(new
            {
                Message = command.IsDraft ? "Job saved as draft successfully." : "Job created successfully.",
                JobId = jobId
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
     Guid id,
     [FromBody] UpdateJobCommand command)
        {
            var recruiterIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(recruiterIdClaim))
                return Unauthorized();

            command.JobId = id;
            command.RecruiterId = Guid.Parse(recruiterIdClaim);

            await _mediator.Send(command);

            return Ok(new
            {
                Message = "Job updated successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var recruiterIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(recruiterIdClaim))
                return Unauthorized();

            await _mediator.Send(
                new DeleteJobCommand
                {
                    JobId = id,
                    RecruiterId = Guid.Parse(recruiterIdClaim)
                });

            return Ok(new
            {
                Message = "Job deleted successfully."
            });
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedJobs(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            return Ok(
                await _mediator.Send(
                    new GetPagedJobsQuery
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }));
        }
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchJobs(
     [FromQuery] string? keyword,
     [FromQuery] string? role,
     [FromQuery] string? location,
     [FromQuery] string? experience,
     [FromQuery] string? workMode,
     [FromQuery] JobType? jobType,
     [FromQuery] decimal? minSalary,
     [FromQuery] decimal? maxSalary,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(
                new SearchJobsQuery
                {
                    Keyword = keyword,
                    Role = role,
                    Location = location,
                    Experience = experience,
                    WorkMode = workMode,
                    JobType = jobType,
                    MinSalary = minSalary,
                    MaxSalary = maxSalary,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

            return Ok(result);
        }

        [HttpGet("filterpreferancedata")]
        [HttpGet("filter-preferences")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFilterPreferences()
        {
            var result = await _mediator.Send(new GetFilterPreferencesQuery());

            return Ok(new
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet("my-jobs")]
        public async Task<IActionResult> GetMyJobs()
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(recruiterIdClaim))
                return Unauthorized();

            var recruiterId = Guid.Parse(recruiterIdClaim);

            var result = await _mediator.Send(
                new GetJobsByRecruiterQuery
                {
                    RecruiterId = recruiterId
                });

            return Ok(result);
        }
        [HttpPut("{id}/renew")]
        public async Task<IActionResult> Renew(Guid id)
        {
            var recruiterIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(recruiterIdClaim))
                return Unauthorized();

            await _mediator.Send(
                new RenewJobCommand
                {
                    JobId = id,
                    RecruiterId = Guid.Parse(recruiterIdClaim)
                });

            return Ok(new
            {
                Message = "Job renewed successfully."
            });
        }
    }
}
