using JobPortal.Application.Features.Auth.Commands.GoogleLogin;
using JobPortal.Application.Features.Auth.Commands.Login;
using JobPortal.Application.Features.Auth.Commands.Register;
using JobPortal.Application.Features.Auth.Commands.RegisterRecruiter;
using JobPortal.Application.Features.Auth.Commands.SendEmailVerification;
using JobPortal.Application.Features.Auth.Commands.VerifyEmail;
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

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(
        //    RegisterCommand command)
        //{
        //    var userId =
        //        await _mediator.Send(command);

        //    return Ok(userId);
        //}
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRecruiterCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginCommand command)
        {
            var response =
                await _mediator.Send(command);

            return Ok(response);
        }
        [HttpPost("send-email-verification")]
        public async Task<IActionResult> SendEmailVerification(
           SendEmailVerificationCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
           VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(
    GoogleLoginCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }
        //[HttpPost("register-recruiter")]
        //public async Task<IActionResult> RegisterRecruiter(
        //    RegisterRecruiterCommand command)
        //{
        //    var result = await _mediator.Send(command);

        //    return Ok(result);
        //}
    }

}

