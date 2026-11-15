using JobPortal.Application.Features.AppInfo.Queries.GetAppInfo;
using JobPortal.Application.Features.AppInfo.Queries.GetPrivacyPolicy;
using JobPortal.Application.Features.AppInfo.Queries.GetTermsAndConditions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortal.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AppInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. GET: /api/AppInfo/about
        [HttpGet("about")]
        public async Task<IActionResult> GetAboutApp()
        {
            var result = await _mediator.Send(new GetAppInfoQuery());
            return Ok(new
            {
                Success = true,
                Data = result
            });
        }

        // 2. GET: /api/AppInfo/privacy-policy
        [HttpGet("privacy-policy")]
        public async Task<IActionResult> GetPrivacyPolicy()
        {
            var result = await _mediator.Send(new GetPrivacyPolicyQuery());
            return Ok(new
            {
                Success = true,
                Data = result
            });
        }

        // 3. GET: /api/AppInfo/terms-and-conditions
        [HttpGet("terms-and-conditions")]
        public async Task<IActionResult> GetTermsAndConditions()
        {
            var result = await _mediator.Send(new GetTermsAndConditionsQuery());
            return Ok(new
            {
                Success = true,
                Data = result
            });
        }

        // 4. GET: /api/AppInfo
        [HttpGet]
        public async Task<IActionResult> GetAppInfoSummary()
        {
            var about = await _mediator.Send(new GetAppInfoQuery());
            var privacy = await _mediator.Send(new GetPrivacyPolicyQuery());
            var terms = await _mediator.Send(new GetTermsAndConditionsQuery());

            return Ok(new
            {
                Success = true,
                Data = new
                {
                    App = about,
                    PrivacyPolicySummary = privacy.Summary,
                    PrivacyPolicyVersion = privacy.Version,
                    TermsSummary = terms.Summary,
                    TermsVersion = terms.Version,
                    LastUpdated = privacy.LastUpdated
                }
            });
        }
    }
}
