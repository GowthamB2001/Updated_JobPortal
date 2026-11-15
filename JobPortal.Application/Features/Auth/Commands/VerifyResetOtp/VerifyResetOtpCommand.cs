using MediatR;

namespace JobPortal.Application.Features.Auth.Commands.VerifyResetOtp
{
    public class VerifyResetOtpCommand : IRequest<VerifyResetOtpResponse>
    {
        public string OfficialEmail { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
