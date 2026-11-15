using JobPortal.Application.Features.Skills.Commands.CreateSkill;
using JobPortal.Application.Features.Skills.Queries.GetAllSkills;
using JobPortal.Application.Features.Skills.Queries.GetSkillById;
using JobPortal.Application.Features.Skills.Commands.UpdateSkill;
using JobPortal.Application.Features.Skills.Commands.DeleteSkill;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public SkillsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateSkillCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _mediator.Send(
                    new GetAllSkillsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetSkillByIdQuery
                {
                    Id = id
                });

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(
    UpdateSkillCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
    Guid id)
        {
            return Ok(
                await _mediator.Send(
                    new DeleteSkillCommand
                    {
                        Id = id
                    }));
        }
    }
}

