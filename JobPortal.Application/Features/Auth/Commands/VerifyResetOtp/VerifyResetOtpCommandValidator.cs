using FluentValidation;

namespace JobPortal.Application.Features.Auth.Commands.VerifyResetOtp
{
    public class VerifyResetOtpCommandValidator : AbstractValidator<VerifyResetOtpCommand>
    {
        public VerifyResetOtpCommandValidator()
        {
            RuleFor(x => x.OfficialEmail)
                .NotEmpty().WithMessage("Official Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required.")
                .Length(6).WithMessage("OTP must be 6 digits.");
        }
    }
}
