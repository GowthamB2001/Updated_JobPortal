using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Dashboard;
using JobPortal.Application.DTOs.Jobs;
using JobPortal.Application.Features.Jobs.Common;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Dashboard.Queries.GetJobSeekerDashboard
{
    public class GetJobSeekerDashboardQueryHandler
        : IRequestHandler<GetJobSeekerDashboardQuery, JobSeekerDashboardDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetJobSeekerDashboardQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JobSeekerDashboardDto> Handle(
            GetJobSeekerDashboardQuery request,
            CancellationToken cancellationToken)
        {
            JobSeekerProfile? profile = null;
            if (request.JobSeekerId.HasValue && request.JobSeekerId.Value != Guid.Empty)
            {
                profile = await _unitOfWork.JobSeekerProfile.GetByIdAsync(request.JobSeekerId.Value);
            }

            var seekerName = !string.IsNullOrWhiteSpace(profile?.Name) ? profile.Name : "Job Seeker";

            // 1. Dynamic Greeting based on current time (IST UTC+5:30)
            var nowUtc = DateTime.UtcNow;
            var currentHour = nowUtc.AddHours(5.5).Hour;
            var greetingText = currentHour < 12
                ? "Good morning"
                : (currentHour < 17 ? "Good afternoon" : "Good evening");

            // 2. Fetch Jobs and Applications
            var allJobs = await _unitOfWork.Jobs.GetAllJobsAsync();
            var allApplications = (await _unitOfWork.JobApplications.GetAllAsync()).ToList();

            // 3. User Applications & Real Stats
            var userApps = profile != null
                ? allApplications.Where(a => a.JobSeekerId == profile.Id).ToList()
                : new List<JobApplication>();

            var appliedJobIds = new HashSet<Guid>(userApps.Select(a => a.JobId));

            // Real count: total jobs the seeker has applied to
            int jobsAppliedCount = userApps.Count;

            // Real count: active jobs posted today (UTC or IST)
            var todayUtc = nowUtc.Date;
            var todayIst = nowUtc.AddHours(5.5).Date;
            int postedTodayCount = allJobs.Count(j => j.IsActive && !j.IsDeleted && (j.PostedOn.Date == todayUtc || j.PostedOn.Date == todayIst));

            // Real count: expired or inactive jobs that the seeker did not apply to
            int jobsMissedCount = allJobs.Count(j => (!j.IsActive || j.ExpiryDate <= nowUtc) && !appliedJobIds.Contains(j.Id));

            // Real count: user's applications that have been opened/reviewed by recruiters
            int jobsViewedCount = userApps.Count(a => a.Status >= Domain.Domain.Enums.ApplicationStatus.Viewed);

            // 4. Banner Details: "Jobs Expiring Soon / Live Now" (within next 24 hours)
            var expiringJobs = allJobs
                .Where(j => j.IsActive && !j.IsDeleted && j.ExpiryDate > nowUtc && j.ExpiryDate <= nowUtc.AddHours(24))
                .OrderBy(j => j.ExpiryDate)
                .ToList();

            int closingJobsCount = expiringJobs.Count;
            TimeSpan shortestExpiry = closingJobsCount > 0
                ? expiringJobs.First().ExpiryDate - nowUtc
                : TimeSpan.Zero;

            if (shortestExpiry < TimeSpan.Zero)
            {
                shortestExpiry = TimeSpan.Zero;
            }

            string remainingTimeStr = $"{(int)shortestExpiry.Hours:D2}:{(int)shortestExpiry.Minutes:D2}:{(int)shortestExpiry.Seconds:D2}";
            string subTag = closingJobsCount == 1 ? "🔥 1 Job Closing Soon" : $"🔥 {closingJobsCount} Jobs Closing Soon";

            IReadOnlyCollection<Guid>? savedJobIds = null;
            if (profile != null)
            {
                savedJobIds = await _unitOfWork.SavedJobs.GetSavedJobIdsAsync(profile.Id);
            }

            // 5. Similar Jobs — only jobs matching seeker's skills or role
            List<JobListDto> similarJobs;
            if (profile != null)
            {
                var seekerSkills = (profile.KeySkills ?? "")
                    .Split(new[] { ',', ';', '/', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().ToLower())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                var seekerRole = profile.JobRole?.Trim().ToLower() ?? "";

                var matchingJobs = allJobs.Where(j =>
                {
                    // Check skill match
                    var jobSkills = j.JobSkills?
                        .Select(x => x.SkillName.Trim().ToLower())
                        .ToList() ?? new List<string>();

                    bool hasSkillMatch = jobSkills.Any(js =>
                        seekerSkills.Any(ss => js.Contains(ss) || ss.Contains(js)));

                    // Check role/title match
                    bool hasRoleMatch = !string.IsNullOrWhiteSpace(seekerRole)
                        && !string.IsNullOrWhiteSpace(j.Title)
                        && (j.Title.Trim().ToLower().Contains(seekerRole)
                            || seekerRole.Contains(j.Title.Trim().ToLower()));

                    return hasSkillMatch || hasRoleMatch;
                }).ToList();

                similarJobs = matchingJobs
                    .Select(j => JobMappingHelper.MapToJobListDto(j, profile, allApplications, savedJobIds))
                    .OrderByDescending(j => j.MatchPercentage)
                    .ThenByDescending(j => j.PostedOn)
                    .ToList();
            }
            else
            {
                similarJobs = new List<JobListDto>();
            }

            // 6. Matches Today Summary (Real count)
            int matchingCount = similarJobs.Count(j => j.MatchPercentage >= 70);
            string matchesSummary = matchingCount switch
            {
                0 => "0 jobs match you today",
                1 => "1 job matches you today",
                _ => $"{matchingCount} jobs match you today"
            };

            // 7. Unread Notifications Count
            int unreadNotifications = 0;
            if (profile != null)
            {
                var userNotifications = await _unitOfWork.Notifications.GetByUserIdAsync(profile.Id);
                unreadNotifications = userNotifications.Count(n => !n.IsRead);
            }

            return new JobSeekerDashboardDto
            {
                Greeting = greetingText,
                JobSeekerName = seekerName,
                MatchesTodaySummary = matchesSummary,
                UnreadNotificationCount = unreadNotifications,

                Stats = new JobSeekerDashboardStatsDto
                {
                    JobsApplied = jobsAppliedCount,
                    PostedToday = postedTodayCount,
                    JobsMissed = jobsMissedCount,
                    JobsViewed = jobsViewedCount
                },

                Banner = new JobSeekerDashboardBannerDto
                {
                    Title = "Jobs Expiring Soon",
                    Tag = "LIVE NOW",
                    SubTag = subTag,
                    ClosingJobsCount = closingJobsCount,
                    RemainingTime = remainingTimeStr,
                    RemainingSeconds = shortestExpiry.TotalSeconds,
                    Description = closingJobsCount > 0
                        ? "Apply before these jobs expire. Don't miss opportunities closing soon."
                        : "No jobs closing within 24 hours. Explore all active opportunities.",
                    ActionText = "Explore Jobs",
                    ActionUrl = "/jobs/expiring"
                },

                SimilarJobs = similarJobs
            };
        }
    }
}
