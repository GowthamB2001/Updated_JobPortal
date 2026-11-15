using JobPortal.Application.DTOs.Legal;
using MediatR;

namespace JobPortal.Application.Features.AppInfo.Queries.GetPrivacyPolicy
{
    public class GetPrivacyPolicyQuery : IRequest<PrivacyPolicyDto>
    {
    }
}
