using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter
{
    public class JobDto
    {
        public Guid Id { get; set; }

        public Guid RecruiterId { get; set; }

        public string? RecruiterType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string SalaryRange { get; set; } = string.Empty;

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }

        public string Currency { get; set; } = "INR";

        public string Experience { get; set; } = string.Empty;

        public JobType JobType { get; set; }

        public string WorkMode { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Openings { get; set; } = string.Empty;

        public string CompanyOverview { get; set; } = string.Empty;

        public int AutoRepostDays { get; set; } = 1;

        public bool IsDraft { get; set; }

        public int RepostCount { get; set; }

        public DateTime PostedOn { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public string Status { get; set; } = string.Empty;

        public int ApplicantCount { get; set; }
        public int AppliedCandidateCount { get; set; }
        public int AppliedCount { get; set; }
        public string ApplicantCountText { get; set; } = string.Empty;

        public List<string> Skills { get; set; } = new();
    }
}
