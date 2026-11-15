using JobPortal.Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class JobSeekerProfile : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Gender { get; set; }

        public string Education { get; set; }

        public string Language { get; set; }

        public string KeySkills { get; set; }

        // Fresher / Experienced
        public string WorkExperience { get; set; }

        // Experienced Only
        public decimal? YearsOfExperience { get; set; }

        public string? LastRoleDesignation { get; set; }

        public decimal? LastMonthSalary { get; set; }

        // Mobile UI Registration Fields
        public string? CurrentLocation { get; set; }
        public string? EmploymentStatus { get; set; }
        public int? NoticePeriod { get; set; }
        public int? TotalExperienceYears { get; set; }
        public int? TotalExperienceMonths { get; set; }
        public int? TotalCompaniesWorked { get; set; }
        public string? CompaniesWorkedJson { get; set; }
        public decimal? LastCompanySalary { get; set; }

        // Common
        public string PreferredLocation { get; set; }

        public string JobRole { get; set; }

        // Fresher Only
        public string? LookingFor { get; set; }

        // Profile Update
        public string? ResumeUrl { get; set; }

        public string Email { get; set; } = string.Empty;
        public ICollection<JobApplication> JobApplications { get; set; }
    = new List<JobApplication>();
    }
}
