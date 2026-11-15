using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.SendEmailVerification
{
    public class SendEmailVerificationCommand : IRequest<bool>
    {
        public string CompanyName { get; set; } = string.Empty;

        public string CompanyEmail { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;
    }
}
