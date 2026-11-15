using JobPortal.Application.DTOs.Legal;
using MediatR;

namespace JobPortal.Application.Features.AppInfo.Queries.GetAppInfo
{
    public class GetAppInfoQuery : IRequest<AppInfoDto>
    {
    }
}
