using JobPortal.Application.DTOs.Jobs;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.DTOs.Dashboard
{
    public class JobSeekerDashboardDto
    {
        public string Greeting { get; set; } = "Good morning";
        public string JobSeekerName { get; set; } = "Praveen";
        public string MatchesTodaySummary { get; set; } = "1 job matches you today";
        public int UnreadNotificationCount { get; set; } = 0;

        public JobSeekerDashboardStatsDto Stats { get; set; } = new();

        public JobSeekerDashboardBannerDto Banner { get; set; } = new();

        public List<JobListDto> SimilarJobs { get; set; } = new();
    }

    public class JobSeekerDashboardStatsDto
    {
        public int JobsApplied { get; set; } = 12;
        public int PostedToday { get; set; } = 47;
        public int JobsMissed { get; set; } = 3;
        public int JobsViewed { get; set; } = 156;
    }

    public class JobSeekerDashboardBannerDto
    {
        public string Title { get; set; } = "Jobs Expiring Soon";
        public string Tag { get; set; } = "LIVE NOW";
        public string SubTag { get; set; } = "24 Jobs Closing Soon";
        public int ClosingJobsCount { get; set; } = 24;
        public string RemainingTime { get; set; } = "00:59:44";
        public double RemainingSeconds { get; set; } = 3584;
        public string Description { get; set; } = "Apply before these jobs expire. Don't miss opportunities closing within the next hour.";
        public string ActionText { get; set; } = "Explore Jobs";
        public string ActionUrl { get; set; } = "/jobs/expiring";
    }
}
