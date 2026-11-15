using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.RegisterRecruiter
{
    public class RegisterRecruiterCommandValidator
            : AbstractValidator<RegisterRecruiterCommand>
    {
        public RegisterRecruiterCommandValidator()
        {
            RuleFor(x => x.OfficialEmail)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.OrganizationType)
                .IsInEnum();

            RuleFor(x => x.RecruiterName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.OrganizationName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .Matches(@"^[6-9]\d{9}$")
                .WithMessage("Invalid Mobile Number");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Password and Confirm Password must match.");
        }
    }
}