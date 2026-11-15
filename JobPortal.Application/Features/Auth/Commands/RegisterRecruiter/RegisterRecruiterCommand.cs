using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.RegisterRecruiter
{
    public class RegisterRecruiterCommand : IRequest<bool>
    {
        public string OfficialEmail { get; set; } = string.Empty;

        public OrganizationType OrganizationType { get; set; }

        public string RecruiterName { get; set; } = string.Empty;

        public string OrganizationName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        // Optional
        public string? EmployeeId { get; set; }

        public string? GstNumber { get; set; }

        public string? Website { get; set; }
    }
}
