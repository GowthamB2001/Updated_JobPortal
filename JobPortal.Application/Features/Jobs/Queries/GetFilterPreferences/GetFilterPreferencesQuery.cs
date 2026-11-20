using JobPortal.Application.DTOs.Jobs;
using MediatR;

namespace JobPortal.Application.Features.Jobs.Queries.GetFilterPreferences
{
    public class GetFilterPreferencesQuery : IRequest<FilterPreferencesDto>
    {
    }
}
