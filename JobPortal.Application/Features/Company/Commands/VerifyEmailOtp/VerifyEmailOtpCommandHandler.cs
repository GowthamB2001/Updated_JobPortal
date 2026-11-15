using JobPortal.Application.Contracts.Persistence;
using MediatR;

namespace JobPortal.Application.Features.Company.Commands.VerifyEmailOtp
{
    public class VerifyEmailOtpCommandHandler
        : IRequestHandler<VerifyEmailOtpCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VerifyEmailOtpCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(
            VerifyEmailOtpCommand request,
            CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies
                .GetByRecruiterIdAsync(request.RecruiterId);

            if (company == null)
                throw new Exception("Company not found.");

            if (company.EmailOtp != request.Otp)
                throw new Exception("Invalid OTP.");

            if (company.OtpExpiry == null ||
                company.OtpExpiry < DateTime.UtcNow)
                throw new Exception("OTP expired.");

            company.IsEmailVerified = true;
            company.EmailOtp = null;
            company.OtpExpiry = null;

            await _unitOfWork.Companies.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}