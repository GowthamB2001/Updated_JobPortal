using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Queries.LoginJobSeeker
{
    public class JobSeekerLoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string MobileNo { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? ResumeUrl { get; set; }

        public string? Gender { get; set; }

        public string? Education { get; set; }

        public string? Language { get; set; }

        public string? KeySkills { get; set; }

        public string? WorkExperience { get; set; }

        public string? YearsOfExperience { get; set; }

        public string? LastRoleDesignation { get; set; }

        public string? LastMonthSalary { get; set; }

        public string? PreferredLocation { get; set; }

        public string? JobRole { get; set; }

        public string? LookingFor { get; set; }
    }
}
