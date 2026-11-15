using JobPortal.Application.Features.Recruiters.Commands.CreateRecruits;
using JobPortal.Application.Features.Recruiters.Queries.GetAllRecruiters;
using JobPortal.Application.Features.Recruiters.Queries.GetRecruiterById;
using JobPortal.Application.Features.Recruiters.Queries.GetMyRecruiterProfile;
using JobPortal.Application.Features.Recruiters.Commands.UpdateRecruiter;
using JobPortal.Application.Features.Recruiters.Commands.DeleteRecruiter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobPortal.WebApi.Controllers
{
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

        [Authorize]
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(recruiterIdClaim) || !Guid.TryParse(recruiterIdClaim, out var recruiterId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid or missing recruiter token claims."
                });
            }

            var result = await _mediator.Send(new GetMyRecruiterProfileQuery
            {
                RecruiterId = recruiterId
            });

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Recruiter profile not found."
                });
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateRecruiterCommand command)
        {
            return Ok(
                await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin,Recruiter")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _mediator.Send(
                    new GetAllRecruitersQuery()));
        }

        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPut]
        public async Task<IActionResult> Update(
    UpdateRecruiterCommand command)
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
                    new DeleteRecruiterCommand
                    {
                        Id = id
                    }));
        }
    }
}

