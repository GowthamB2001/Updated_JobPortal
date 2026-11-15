using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<bool>
    {
        public string CompanyEmail { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
