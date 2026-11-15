using FluentValidation;

namespace JobPortal.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.OfficialEmail)
                .NotEmpty().WithMessage("Official Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
        }
    }
}
