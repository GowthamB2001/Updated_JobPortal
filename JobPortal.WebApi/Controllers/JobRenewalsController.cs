using JobPortal.Application.Features.JobRenewals.Commands.Queries.GetRenewalByJob;
using JobPortal.Application.Features.JobRenewals.Commands.RenewJob;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRenewalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobRenewalsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> RenewJob(
            RenewJobCommand command)
        {
            var result =
                await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetByJob(
            Guid jobId)
        {
            var result =
                await _mediator.Send(
                    new GetRenewalByJobQuery
                    {
                        JobId = jobId
                    });

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }

}
