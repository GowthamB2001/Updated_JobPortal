using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Dashboard;
using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Dashboard.Queries.GetDashboardStats
{
    public class GetDashboardStatsQueryHandler
       : IRequestHandler<
           GetDashboardStatsQuery,
           DashboardStatsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDashboardStatsQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardStatsDto> Handle(
            GetDashboardStatsQuery request,
            CancellationToken cancellationToken)
        {
            var users =
                await _unitOfWork.Users.GetAllAsync();

            var recruiters =
                await _unitOfWork.Recruiters.GetAllAsync();

            var jobSeekers =
                await _unitOfWork.JobSeekers.GetAllAsync();

            var jobs =
                await _unitOfWork.Jobs.GetAllAsync();

            var applications =
                await _unitOfWork.JobApplications.GetAllAsync();

            var skills =
                await _unitOfWork.Skills.GetAllAsync();

            var resumes =
                await _unitOfWork.Resumes.GetAllAsync();

            var renewals =
                await _unitOfWork.JobRenewals.GetAllAsync();
            var interviews =
    await _unitOfWork.Interviews
        .GetAllAsync();

            return new DashboardStatsDto
            {
                TotalUsers = users.Count(),
                TotalRecruiters = recruiters.Count(),
                TotalJobSeekers = jobSeekers.Count(),
                TotalJobs = jobs.Count(),
                TotalApplications = applications.Count(),
                TotalSkills = skills.Count(),
                TotalResumes = resumes.Count(),
                TotalJobRenewals = renewals.Count(),
                TotalInterviews =
    interviews.Count(),

                ScheduledInterviews =
    interviews.Count(x =>
        x.Status ==
        InterviewStatus.Scheduled),

                AcceptedInterviews =
    interviews.Count(x =>
        x.Status ==
        InterviewStatus.Accepted),

                RejectedInterviews =
    interviews.Count(x =>
        x.Status ==
        InterviewStatus.Rejected),

                CompletedInterviews =
    interviews.Count(x =>
        x.Status ==
        InterviewStatus.Completed),

                CancelledInterviews =
    interviews.Count(x =>
        x.Status ==
        InterviewStatus.Cancelled),
            };
        }
    }
}
