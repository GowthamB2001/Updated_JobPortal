using JobPortal.Application.Features.Interviews.Commands.AcceptInterview;
using JobPortal.Application.Features.Interviews.Commands.CreateInterview;
using JobPortal.Application.Features.Interviews.Commands.RejectInterview;
using JobPortal.Application.Features.Interviews.Commands.UpdateInterviewStatus;
using JobPortal.Application.Features.Interviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InterviewsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateInterviewCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetByApplication(
            Guid applicationId)
        {
            return Ok(
                await _mediator.Send(
                    new GetInterviewsByApplicationQuery
                    {
                        JobApplicationId =
                            applicationId
                    }));
        }
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(
    UpdateInterviewStatusCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [HttpPut("accept/{id}")]
        public async Task<IActionResult> Accept(
    Guid id)
        {
            return Ok(
                await _mediator.Send(
                    new AcceptInterviewCommand
                    {
                        InterviewId = id
                    }));
        }
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(
    Guid id)
        {
            return Ok(
                await _mediator.Send(
                    new RejectInterviewCommand
                    {
                        InterviewId = id
                    }));
        }
    }

}
