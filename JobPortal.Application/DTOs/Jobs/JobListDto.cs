using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.DTOs.Jobs
{
    public class JobListDto
    {
        public Guid Id { get; set; }

        // Header & Company info (Screenshots 2 & 3)
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string CompanyInitial => !string.IsNullOrWhiteSpace(Company) ? Company.Substring(0, 1).ToUpper() : "C";
        public string? CompanyLogo { get; set; }
        public string? CompanyOverview { get; set; }
        public string? CompanyProfile { get; set; }

        // Match Score & Badges (Screenshots 2 & 3)
        public int MatchPercentage { get; set; } = 70;
        public string MatchPercentageText => $"{MatchPercentage}% match";
        public string ProfileMatchText => $"Profile Match {MatchPercentage}%";
        public string ProfileMatchDescription { get; set; } = "Your skills align well with this opportunity.";
        public bool IsActiveRecruiter { get; set; } = true;
        public bool FastReply { get; set; } = true;
        public string ReplyRate { get; set; } = "92% reply rate";
        public bool IsActivelyHiring { get; set; } = true;
        public bool IsBookmarked { get; set; } = false;

        // Compensation & Requirements (Screenshots 2, 3, 4)
        public string SalaryRange { get; set; } = string.Empty;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string Currency { get; set; } = "INR";
        public string Experience { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public string JobTypeText => JobType switch
        {
            JobType.FullTime => "Full Time",
            JobType.PartTime => "Part Time",
            JobType.Contract => "Contract",
            JobType.Internship => "Internship",
            _ => JobType.ToString()
        };
        public string WorkMode { get; set; } = string.Empty;
        public string Openings { get; set; } = "01";
        public List<string> KeySkills { get; set; } = new();

        // Timing & Applicants (Screenshots 3 & 4)
        public int ApplicantCount { get; set; } = 0;
        public string ApplicantCountText => $"{ApplicantCount} Applicants";
        public DateTime PostedOn { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ClosesIn { get; set; } = "18:41:57";
        public double RemainingSeconds { get; set; }
        public bool HasApplied { get; set; } = false;

        // Detailed descriptions (Screenshots 3 & 4)
        public string JobDescription { get; set; } = string.Empty;
        public string AboutTheRole => JobDescription;
    }
}
