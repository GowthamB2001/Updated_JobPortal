using JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker;
using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using JobPortal.Application.Features.RegisterJobSeeker.Command.UpdateJobSeekerProfile;
using JobPortal.Application.Features.RegisterJobSeeker.Queries.GetAllJobSeekerProfiles;
using JobPortal.Application.Features.RegisterJobSeeker.Command.DeleteJobSeekerProfile;
using JobPortal.Application.Features.RegisterJobSeeker.Queries.LoginJobSeeker;
using JobPortal.Application.Features.Jobs.Queries.SearchJobs;
using JobPortal.Application.Features.Jobs.Queries.GetAllJobs;

namespace JobPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobSeekerProfilesController(IMediator mediator) { 
        
            _mediator = mediator;
        
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllJobSeekerProfilesQuery());

            return Ok(result);
        }
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register(
     [FromForm] RegisterJobSeekerCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                Success = true,
                Message = "Job seeker profile completed successfully.",
                JobSeekerId = id,
                IsProfileCompleted = true
            });
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
    Guid id,
    [FromForm] UpdateJobSeekerProfileCommand command)
        {
            command.Id = id;

            await _mediator.Send(command);

            return Ok(new
            {
                Success = true,
                Message = "Profile updated successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(
                new DeleteJobSeekerProfileCommand
                {
                    Id = id
                });

            return Ok(new
            {
                Success = true,
                Message = "Account deleted successfully."
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JobSeekerLoginQuery query)
        {
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Mobile number not registered."
                });
            }

            return Ok(new
            {
                Success = true,
                User = result
            });
        }
        [HttpGet("JobsList")]
        public async Task<IActionResult> GetAllJobs()
        {
            var result = await _mediator.Send(
                new SearchJobsQuery
                {
                    PageNumber = 1,
                    PageSize = 100
                });

            return Ok(result);
        }
        [HttpGet("Fulljoblist")]
        public async Task<IActionResult> GetAllJobList()
        {
            var result = await _mediator.Send(new GetAllJobsQuery());

            return Ok(new
            {
                Success = true,
                Jobs = result
            });
        }
    }
}
