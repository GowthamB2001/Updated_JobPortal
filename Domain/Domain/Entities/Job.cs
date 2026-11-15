using JobPortal.Domain.Domain.Common;
using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class Job : BaseEntity
    {
        public Guid RecruiterId { get; set; }

        public Recruiter Recruiter { get; set; } = null!;

        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string SalaryRange { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public JobType JobType { get; set; }

        public string WorkMode { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Openings { get; set; } = string.Empty;

        public string CompanyOverview { get; set; } = string.Empty;

        public int RepostCount { get; set; } = 0;
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime PostedOn { get; set; } = DateTime.UtcNow;

        public ICollection<JobSkill> JobSkills { get; set; }
            = new List<JobSkill>();
        public ICollection<JobApplication> JobApplications { get; set; }
           = new List<JobApplication>();
    }
}
