using JobPortal.Application.DTOs.Legal;
using MediatR;

namespace JobPortal.Application.Features.AppInfo.Queries.GetTermsAndConditions
{
    public class GetTermsAndConditionsQuery : IRequest<TermsAndConditionsDto>
    {
    }
}
