using MediatR;

namespace JobPortal.Application.Features.Company.Commands.SendCompanyEmailOtp
{
    public class SendCompanyEmailOtpCommand : IRequest<Unit>
    {
        public Guid RecruiterId { get; set; }
    }
}