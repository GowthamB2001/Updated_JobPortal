using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;

namespace JobPortal.Application.Features.Auth.Commands.RegisterRecruiter
{
    public class RegisterRecruiterCommand : IRequest<Guid>
    {
        // Step 1 - Company Verification Fields
        public string? CompanyName { get; set; }
        public string? OrganizationName { get; set; }
        public string OfficialEmail { get; set; } = string.Empty;
        public string? CompanyEmail { get; set; }
        public string MobileNumber { get; set; } = string.Empty;

        // Step 2 - Recruiter Information Fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Designation { get; set; }
        public string? JobTitle { get; set; }
        public string? RecruiterName { get; set; }

        // Recruiter Type (1 = In-house HR / OwnCompany, 2 = HR Consultancy)
        public OrganizationType OrganizationType { get; set; } = OrganizationType.OwnCompany;

        // Passwords
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        // Optional / Backward Compatibility Fields
        public string? Address { get; set; }
        public string? EmployeeId { get; set; }
        public string? GstNumber { get; set; }
        public string? Website { get; set; }
    }
}
