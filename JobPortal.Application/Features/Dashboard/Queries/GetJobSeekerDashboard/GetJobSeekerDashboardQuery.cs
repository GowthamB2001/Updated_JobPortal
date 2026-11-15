using JobPortal.Application.DTOs.Dashboard;
using MediatR;
using System;

namespace JobPortal.Application.Features.Dashboard.Queries.GetJobSeekerDashboard
{
    public class GetJobSeekerDashboardQuery : IRequest<JobSeekerDashboardDto>
    {
        public Guid? JobSeekerId { get; set; }
    }
}
