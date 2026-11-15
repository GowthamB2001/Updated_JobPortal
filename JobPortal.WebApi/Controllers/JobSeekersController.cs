using JobPortal.Application.Features.JobSeekers.Commands.CreateJobSeeker;
using JobPortal.Application.Features.JobSeekers.Queries.GetAllJobSeekers;
using JobPortal.Application.Features.JobSeekers.Queries.GetJobSeekerById;
using JobPortal.Application.Features.JobSeekers.Commands.UpdateJobSeeker;
using JobPortal.Application.Features.JobSeekers.Commands.DeleteJobSeeker;
using JobPortal.Application.Features.JobSeekerSkills.Commands.AddSkillToJobSeeker;
using JobPortal.Application.Features.JobSeekerSkills.Commands.RemoveSkillFromJobSeeker;
using JobPortal.Application.Features.JobSeekerSkills.Queries.GetSkillsByJobSeeker;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobSeekersController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin,JobSeeker")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateJobSeekerCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _mediator.Send(
                    new GetAllJobSeekersQuery()));
        }

        [Authorize(Roles = "Admin,Recruiter,JobSeeker")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            Guid id)
        {
            return Ok(
                await _mediator.Send(
                    new GetJobSeekerByIdQuery
                    {
                        Id = id
                    }));
        }

        [Authorize(Roles = "Admin,JobSeeker")]
        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateJobSeekerCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            return Ok(
                await _mediator.Send(
                    new DeleteJobSeekerCommand
                    {
                        Id = id
                    }));
        }
    
}
}

