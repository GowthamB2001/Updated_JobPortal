using MediatR;

namespace JobPortal.Application.Features.Company.Commands.VerifyEmailOtp
{
    public class VerifyEmailOtpCommand : IRequest<Unit>
    {
        public Guid RecruiterId { get; set; }
        public string Otp { get; set; } = string.Empty;
    }
}