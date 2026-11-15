using JobPortal.Application.DTOs.Dashboard;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Dashboard.Queries.GetDashboardStats
{
    public class GetDashboardStatsQuery
        : IRequest<DashboardStatsDto>
    {
    }
}
