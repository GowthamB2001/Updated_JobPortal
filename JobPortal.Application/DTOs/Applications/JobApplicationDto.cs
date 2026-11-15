using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.DTOs.Applications
{
    public class JobApplicationDto
    {
        // Application Identifiers
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid JobSeekerId { get; set; }

        // ================= Screenshot 1, 2, 3, 4: Job Details =================
        public string? JobTitle { get; set; }
        public string Company { get; set; } = string.Empty;
        public string CompanyInitial { get; set; } = "C";
        public string? CompanyLogo { get; set; }
        public string JobLocation { get; set; } = string.Empty;
        public string Location => JobLocation;

        // Screenshot 2: Badges
        public bool IsActiveRecruiter { get; set; } = true;
        public string ReplyRate { get; set; } = "92% reply rate";
        public bool IsBookmarked { get; set; } = false;

        // Screenshot 2 & 3: 4 Quick Stat Cards
        public string SalaryRange { get; set; } = string.Empty;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string Currency { get; set; } = "INR";
        public string Experience { get; set; } = string.Empty;
        public string JobType { get; set; } = "Full-time";
        public string JobTypeText => JobType;
        public string WorkMode { get; set; } = "Hybrid";
        public string Openings { get; set; } = "01";

        // Screenshot 2: Applicant Count
        public int ApplicantCount { get; set; } = 42;
        public string ApplicantCountText => $"{ApplicantCount} Applicants";

        // ================= Screenshot 1 & 2: Application Status & Cards =================
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public string StatusText => Status.ToString();

        // Screenshot 2: Green Confirmation Banner
        public string StatusBannerTitle { get; set; } = "Great work! You applied on time.";
        public string StatusBannerDescription { get; set; } = "The recruiter will review your profile and contact you shortly regarding the next steps.";

        // Screenshot 1 & 2: Dates & Relative Time
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
        public string AppliedOnFormatted => AppliedDate.ToString("MMM dd, yyyy");
        public string AppliedTimeAgo { get; set; } = "Applied recently";

        // Screenshot 2, 3, 4: Bottom CTA
        public bool HasApplied { get; set; } = true;

        // Screenshot 2 & 3: About the Role & Skills Required
        public string AboutTheRole { get; set; } = string.Empty;
        public string JobDescription => AboutTheRole;
        public List<string> SkillsRequired { get; set; } = new();

        // Screenshot 4: Company Overview
        public string CompanyOverview { get; set; } = string.Empty;
        public string? CompanyProfile { get; set; }

        // ================= Candidate Profile Snapshot =================
        public string Name { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string KeySkills { get; set; } = string.Empty;
        public string WorkExperience { get; set; } = string.Empty;
        public decimal? YearsOfExperience { get; set; }
        public string PreferredLocation { get; set; } = string.Empty;
        public string JobRole { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string? CoverLetter { get; set; }
        public string? RecruiterRemarks { get; set; }
    }
}
