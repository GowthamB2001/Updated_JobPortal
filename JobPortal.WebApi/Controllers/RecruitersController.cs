using JobPortal.Application.Features.Recruiters.Commands.CreateRecruits;
using JobPortal.Application.Features.Recruiters.Queries.GetAllRecruiters;
using JobPortal.Application.Features.Recruiters.Queries.GetRecruiterById;
using JobPortal.Application.Features.Recruiters.Commands.UpdateRecruiter;
using JobPortal.Application.Features.Recruiters.Commands.DeleteRecruiter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecruitersController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateRecruiterCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            Guid id)
        {
            return Ok(
                await _mediator.Send(
                    new GetRecruiterByIdQuery
                    {
                        Id = id
                    }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _mediator.Send(
                    new GetAllRecruitersQuery()));
        }
        [HttpPut]
        public async Task<IActionResult> Update(
    UpdateRecruiterCommand command)
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
                    new DeleteRecruiterCommand
                    {
                        Id = id
                    }));
        }
    }
}

