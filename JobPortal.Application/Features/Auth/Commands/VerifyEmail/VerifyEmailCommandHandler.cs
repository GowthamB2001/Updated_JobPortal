using JobPortal.Application.Contracts.Persistence;
using MediatR;

namespace JobPortal.Application.Features.Auth.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler
        : IRequestHandler<VerifyEmailCommand, bool>
    {
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyEmailCommandHandler(
            IEmailVerificationRepository emailVerificationRepository,
            IUnitOfWork unitOfWork)
        {
            _emailVerificationRepository = emailVerificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            VerifyEmailCommand request,
            CancellationToken cancellationToken)
        {
            var verification = await _emailVerificationRepository
                .GetByCompanyEmailAsync(request.CompanyEmail);

            if (verification == null)
                throw new Exception("Verification request not found.");

            if (verification.IsVerified)
                throw new Exception("Email already verified.");

            if (verification.Otp != request.Otp)
                throw new Exception("Invalid OTP.");

            if (verification.ExpiresAt < DateTime.UtcNow)
                throw new Exception("OTP has expired.");

            verification.IsVerified = true;

            await _emailVerificationRepository.UpdateAsync(verification);

            // IMPORTANT
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}