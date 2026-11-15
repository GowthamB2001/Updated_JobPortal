using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Company.Commands.SendCompanyEmailOtp
{
    public class SendCompanyEmailOtpCommandValidator
         : AbstractValidator<SendCompanyEmailOtpCommand>
    {
        public SendCompanyEmailOtpCommandValidator()
        {
            RuleFor(x => x.RecruiterId)
                .NotEmpty()
                .WithMessage("Recruiter Id is required.");
        }
    }
}
