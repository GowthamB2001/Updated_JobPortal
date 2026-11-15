using FluentValidation;

namespace JobPortal.Application.Features.Auth.Commands.RegisterRecruiter
{
    public class RegisterRecruiterCommandValidator
            : AbstractValidator<RegisterRecruiterCommand>
    {
        public RegisterRecruiterCommandValidator()
        {
            RuleFor(x => x.OfficialEmail)
                .NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.CompanyEmail))
                .WithMessage("Official Company Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.OrganizationType)
                .IsInEnum().WithMessage("A valid recruiter organization type must be selected.");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.RecruiterName) || !string.IsNullOrWhiteSpace(x.FirstName))
                .WithMessage("First Name or Recruiter Name is required.");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.OrganizationName) || !string.IsNullOrWhiteSpace(x.CompanyName))
                .WithMessage("Company Name is required.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile Number is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Password and Confirm Password must match.");
        }
    }
}
