using System.Security.Claims;
using JobPortal.Application.Features.Dashboard.Queries.GetDashboardStats;
using JobPortal.Application.Features.Dashboard.Queries.GetJobSeekerDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            return Ok(
                await _mediator.Send(
                    new GetDashboardStatsQuery()));
        }

        [HttpGet("jobseeker")]
        public async Task<IActionResult> GetJobSeekerDashboard()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;
            Guid? seekerId = Guid.TryParse(userIdClaim, out var id) ? id : null;

            var result = await _mediator.Send(new GetJobSeekerDashboardQuery { JobSeekerId = seekerId });

            return Ok(new
            {
                Success = true,
                Data = result
            });
        }
    }
}
