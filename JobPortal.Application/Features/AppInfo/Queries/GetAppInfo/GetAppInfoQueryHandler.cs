using JobPortal.Application.DTOs.Legal;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.AppInfo.Queries.GetAppInfo
{
    public class GetAppInfoQueryHandler : IRequestHandler<GetAppInfoQuery, AppInfoDto>
    {
        public Task<AppInfoDto> Handle(GetAppInfoQuery request, CancellationToken cancellationToken)
        {
            var appInfo = new AppInfoDto();
            return Task.FromResult(appInfo);
        }
    }
}
