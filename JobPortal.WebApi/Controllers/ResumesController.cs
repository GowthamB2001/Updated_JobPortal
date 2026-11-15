using JobPortal.Application.Features.Resumes.Commands.UploadResume;
using JobPortal.Application.Features.Resumes.Queries.GetResumesByJobSeeker;
using JobPortal.Application.Features.Resumes.Commands.DeleteResume;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.WebApi.Controllers
{
    [Authorize(Roles = "JobSeeker")]
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResumesController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            [FromForm] UploadResumeCommand command)
        {
            var id =
                await _mediator.Send(command);

            return Ok(id);
        }
        [HttpGet("jobseeker/{jobSeekerId}")]
        public async Task<IActionResult> GetByJobSeeker(
    Guid jobSeekerId)
        {
            var result =
                await _mediator.Send(
                    new GetResumesByJobSeekerQuery
                    {
                        JobSeekerId = jobSeekerId
                    });

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
    Guid id)
        {
            return Ok(
                await _mediator.Send(
                    new DeleteResumeCommand
                    {
                        Id = id
                    }));
        }
    }
}
