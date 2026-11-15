using JobPortal.Application.Features.JobApplications.Commands.ApplyJob;
using MediatR;
using JobPortal.Application.Features.JobApplications.Commands.UpdateApplicationStatus;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationById;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJob;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJobSeeker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.WebApi.Controllers
{
    [Authorize(Roles = "JobSeeker")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result =
                await _mediator.Send(
                    new GetApplicationByIdQuery
                    {
                        Id = id
                    });

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJob(
    Guid jobId)
        {
            return Ok(
                await _mediator.Send(
                    new GetApplicationsByJobQuery
                    {
                        JobId = jobId
                    }));
        }
        [HttpGet("jobseeker/{jobSeekerId}")]
        public async Task<IActionResult> GetByJobSeeker(
    Guid jobSeekerId)
        {
            return Ok(
                await _mediator.Send(
                    new GetApplicationsByJobSeekerQuery
                    {
                        JobSeekerId = jobSeekerId
                    }));
        }
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(
    UpdateApplicationStatusCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyJob(
             [FromBody] ApplyJobCommand command)
        {
            // JWT-la JobSeekerId irundha idha use pannunga
            // command.JobSeekerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

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
