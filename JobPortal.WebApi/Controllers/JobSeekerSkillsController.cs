using JobPortal.Application.Features.JobSeekerSkills.Commands.AddSkillToJobSeeker;
using JobPortal.Application.Features.JobSeekerSkills.Commands.RemoveSkillFromJobSeeker;
using JobPortal.Application.Features.JobSeekerSkills.Queries.GetSkillsByJobSeeker;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerSkillsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobSeekerSkillsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin,JobSeeker")]
        [HttpPost]
        public async Task<IActionResult> AddSkill(
            AddSkillToJobSeekerCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin,JobSeeker")]
        [HttpDelete]
        public async Task<IActionResult> RemoveSkill(
            RemoveSkillFromJobSeekerCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin,Recruiter,JobSeeker")]
        [HttpGet("{jobSeekerId}")]
        public async Task<IActionResult> GetSkills(
            Guid jobSeekerId)
        {
            return Ok(
                await _mediator.Send(
                    new GetSkillsByJobSeekerQuery
                    {
                        JobSeekerId = jobSeekerId
                    }));
        }
    
}
}
