using JobPortal.Domain.Domain.Common;
using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        // Job Details
        public Guid JobId { get; set; }
        public Job Job { get; set; } = null!;

        // Job Seeker Details
        public Guid JobSeekerId { get; set; }
        public JobSeekerProfile JobSeekerProfile { get; set; } = null!;

        // Candidate Snapshot
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

        // Application Status
        public ApplicationStatus Status { get; set; }
            = ApplicationStatus.Pending;

        public DateTime AppliedOn { get; set; }
            = DateTime.UtcNow;
    }
}
