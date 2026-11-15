using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }

        public int TotalRecruiters { get; set; }

        public int TotalJobSeekers { get; set; }

        public int TotalJobs { get; set; }

        public int TotalApplications { get; set; }

        public int TotalSkills { get; set; }

        public int TotalResumes { get; set; }

        public int TotalJobRenewals { get; set; }
        public int TotalInterviews { get; set; }

        public int ScheduledInterviews { get; set; }

        public int AcceptedInterviews { get; set; }

        public int RejectedInterviews { get; set; }

        public int CompletedInterviews { get; set; }

        public int CancelledInterviews { get; set; }
    }
}
