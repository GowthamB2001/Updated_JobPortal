using JobPortal.Domain.Domain.Common;
using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;

namespace JobPortal.Domain.Domain.Entities
{
    public class Recruiter : BaseEntity
    {
        // Login
        public string OfficialEmail { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        // Own Company / Consultancy (In-house HR = 1, Consultancy = 2)
        public OrganizationType OrganizationType { get; set; }

        // Recruiter Details
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string RecruiterName { get; set; } = string.Empty;

        public string? Designation { get; set; }

        public string OrganizationName { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        // Optional
        public string? EmployeeId { get; set; }

        public string? GstNumber { get; set; }

        public string? Website { get; set; }

        // Navigation
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
