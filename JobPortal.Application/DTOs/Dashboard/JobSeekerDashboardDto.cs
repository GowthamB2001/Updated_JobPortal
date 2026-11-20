using JobPortal.Application.DTOs.Jobs;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.DTOs.Dashboard
{
    public class JobSeekerDashboardDto
    {
        public string Greeting { get; set; } = "Good morning";
        public string JobSeekerName { get; set; } = "Job Seeker";
        public string MatchesTodaySummary { get; set; } = "0 jobs match you today";
        public int UnreadNotificationCount { get; set; } = 0;

        public JobSeekerDashboardStatsDto Stats { get; set; } = new();

        public JobSeekerDashboardBannerDto Banner { get; set; } = new();

        public List<JobListDto> SimilarJobs { get; set; } = new();
    }

    public class JobSeekerDashboardStatsDto
    {
        public int JobsApplied { get; set; } = 0;
        public int PostedToday { get; set; } = 0;
        public int JobsMissed { get; set; } = 0;
        public int JobsViewed { get; set; } = 0;
    }

    public class JobSeekerDashboardBannerDto
    {
        public string Title { get; set; } = "Jobs Expiring Soon";
        public string Tag { get; set; } = "LIVE NOW";
        public string SubTag { get; set; } = "0 Jobs Closing Soon";
        public int ClosingJobsCount { get; set; } = 0;
        public string RemainingTime { get; set; } = "00:00:00";
        public double RemainingSeconds { get; set; } = 0;
        public string Description { get; set; } = "Apply before these jobs expire. Don't miss opportunities closing soon.";
        public string ActionText { get; set; } = "Explore Jobs";
        public string ActionUrl { get; set; } = "/jobs/expiring";
    }
}
