using JobPortal.Application.Features.Company.Commands.CreateCompany;
using JobPortal.Application.Features.Company.Commands.SendCompanyEmailOtp;
using JobPortal.Application.Features.Company.Commands.VerifyEmailOtp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace JobPortal.WebApi.Controllers
{
    [Authorize(Roles = "Recruiter")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyCommand command)
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (recruiterIdClaim == null)
                return Unauthorized();

            command.RecruiterId = Guid.Parse(recruiterIdClaim.Value);

            var companyId = await _mediator.Send(command);

            return Ok(new
            {
                CompanyId = companyId,
                Message = "Company created successfully."
            });
        }
        [HttpPost("send-email-otp")]
        public async Task<IActionResult> SendEmailOtp()
        {
            var recruiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _mediator.Send(new SendCompanyEmailOtpCommand
            {
                RecruiterId = recruiterId
            });

            return Ok(new
            {
                Message = "OTP sent successfully."
            });
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
    VerifyEmailOtpCommand command)
        {
            command.RecruiterId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _mediator.Send(command);

            return Ok(new
            {
                Message = "Email verified successfully."
            });
        }
    }
}
