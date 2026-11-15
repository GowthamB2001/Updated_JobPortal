using JobPortal.Domain.Domain.Common;
using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class Recruiter : BaseEntity
    {
        // Login
        public string OfficialEmail { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        // Own Company / Consultancy
        public OrganizationType OrganizationType { get; set; }

        // Recruiter Details
        public string RecruiterName { get; set; } = string.Empty;

        public string OrganizationName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        // Optional
        public string? EmployeeId { get; set; }

        public string? GstNumber { get; set; }

        public string? Website { get; set; }

        // Navigation
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
