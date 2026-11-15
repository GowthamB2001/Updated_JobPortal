using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.VerifyResetOtp
{
    public class VerifyResetOtpCommandHandler
        : IRequestHandler<VerifyResetOtpCommand, VerifyResetOtpResponse>
    {
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyResetOtpCommandHandler(
            IEmailVerificationRepository emailVerificationRepository,
            IUnitOfWork unitOfWork)
        {
            _emailVerificationRepository = emailVerificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<VerifyResetOtpResponse> Handle(
            VerifyResetOtpCommand request,
            CancellationToken cancellationToken)
        {
            var verification = await _emailVerificationRepository
                .GetByCompanyEmailAsync(request.OfficialEmail);

            if (verification == null)
                throw new Exception("No password reset request found for this email.");

            if (verification.ExpiresAt < DateTime.UtcNow)
                throw new Exception("The OTP has expired. Please request a new password reset OTP.");

            if (verification.Otp != request.Otp)
                throw new Exception("Invalid OTP. Please check the code and try again.");

            verification.IsVerified = true;
            await _emailVerificationRepository.UpdateAsync(verification);
            await _unitOfWork.SaveChangesAsync();

            return new VerifyResetOtpResponse
            {
                Success = true,
                Message = "OTP verified successfully. You may now reset your password."
            };
        }
    }
}
