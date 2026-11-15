using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.SendEmailVerification
{
    public class SendEmailVerificationCommandValidator
        : AbstractValidator<SendEmailVerificationCommand>
    {
        public SendEmailVerificationCommandValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.CompanyEmail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .Matches(@"^[6-9]\d{9}$")
                .WithMessage("Invalid mobile number.");
        }
    }
}
