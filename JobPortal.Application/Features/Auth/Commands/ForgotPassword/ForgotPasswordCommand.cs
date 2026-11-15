using MediatR;

namespace JobPortal.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ForgotPasswordResponse>
    {
        public string OfficialEmail { get; set; } = string.Empty;
    }
}
