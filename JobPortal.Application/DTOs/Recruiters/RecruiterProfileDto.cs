using System;

namespace JobPortal.Application.DTOs.Recruiters
{
    public class RecruiterProfileDto
    {
        public Guid Id { get; set; }

        // Recruiter Name & Designation
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Designation { get; set; }
        public string FullName { get; set; } = string.Empty;

        // Contact & Company Details
        public string OfficialEmail { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyType { get; set; } = string.Empty;
        public int OrganizationType { get; set; }
        public string CompanyLocation { get; set; } = string.Empty;
        public string? EmployeeId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string AccountStatus { get; set; } = "Active";

        // Company Details & Overview
        public string? CompanyOverview { get; set; }
        public string? Website { get; set; }
        public string? GstNumber { get; set; }

        // Credits
        public int AvailableCredits { get; set; } = 10;

        // Legal & Policies
        public string PrivacyPolicyUrl { get; set; } = "/legal/privacy-policy";
        public string TermsAndConditionsUrl { get; set; } = "/legal/terms-and-conditions";

        // Stats
        public int TotalJobsPosted { get; set; }
        public int ActiveJobs { get; set; }
    }
}
