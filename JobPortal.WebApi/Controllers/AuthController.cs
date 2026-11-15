using JobPortal.Application.Features.Auth.Commands.GoogleLogin;
using JobPortal.Application.Features.Auth.Commands.Login;
using JobPortal.Application.Features.Auth.Commands.Register;
using JobPortal.Application.Features.Auth.Commands.RegisterRecruiter;
using JobPortal.Application.Features.Auth.Commands.SendEmailVerification;
using JobPortal.Application.Features.Auth.Commands.VerifyEmail;
using JobPortal.Application.Features.Auth.Commands.ForgotPassword;
using JobPortal.Application.Features.Auth.Commands.VerifyResetOtp;
using JobPortal.Application.Features.Auth.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
  
namespace JobPortal.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRecruiterCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                Success = true,
                Message = "Recruiter registered successfully.",
                RecruiterId = result
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost("send-email-verification")]
        public async Task<IActionResult> SendEmailVerification(SendEmailVerificationCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("verify-reset-otp")]
        public async Task<IActionResult> VerifyResetOtp([FromBody] VerifyResetOtpCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
