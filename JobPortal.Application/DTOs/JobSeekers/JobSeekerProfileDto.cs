using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.JobSeekers
{
    public class JobSeekerProfileDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string MobileNo { get; set; }

        public string Gender { get; set; }

        public string Education { get; set; }

        public string Language { get; set; }

        public string KeySkills { get; set; }

        public string WorkExperience { get; set; }

        public decimal? YearsOfExperience { get; set; }

        public string? LastRoleDesignation { get; set; }

        public decimal? LastMonthSalary { get; set; }

        public string PreferredLocation { get; set; }

        public string JobRole { get; set; }

        public string? LookingFor { get; set; }

        public string? ResumeUrl { get; set; }

        public string? Email { get; set; }
    }
}
