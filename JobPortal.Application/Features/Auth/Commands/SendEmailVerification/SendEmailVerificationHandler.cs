using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Models.Email;
using JobPortal.Domain.Domain.Entities;
using MediatR;

namespace JobPortal.Application.Features.Auth.Commands.SendEmailVerification
{
    public class SendEmailVerificationCommandHandler
        : IRequestHandler<SendEmailVerificationCommand, bool>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public SendEmailVerificationCommandHandler(
            ICompanyRepository companyRepository,
            IEmailVerificationRepository emailVerificationRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            SendEmailVerificationCommand request,
            CancellationToken cancellationToken)
        {
            var company = await _companyRepository
                .GetByOfficialEmailAsync(request.CompanyEmail);

            if (company != null)
                throw new Exception("Company email is already registered.");

            var otp = Random.Shared.Next(100000, 999999).ToString();

            var verification = await _emailVerificationRepository
                .GetByCompanyEmailAsync(request.CompanyEmail);

            if (verification == null)
            {
                verification = new EmailVerification
                {
                    CompanyEmail = request.CompanyEmail,
                    Otp = otp,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                    IsVerified = false
                };

                await _emailVerificationRepository.AddAsync(verification);
            }
            else
            {
                verification.Otp = otp;
                verification.CreatedAt = DateTime.UtcNow;
                verification.ExpiresAt = DateTime.UtcNow.AddMinutes(5);
                verification.IsVerified = false;

                await _emailVerificationRepository.UpdateAsync(verification);
            }

            // IMPORTANT
            await _unitOfWork.SaveChangesAsync();

            await _emailService.SendEmailAsync(new EmailMessage
            {
                To = request.CompanyEmail,
                Subject = "Interview Spot - Email Verification Code",
                Body = $@"
                    <h2>Email Verification</h2>
                    <p>Hello <b>{request.CompanyName}</b>,</p>

                    <p>Your verification code is:</p>

                    <h1 style='letter-spacing:5px'>{otp}</h1>

                    <p>This OTP is valid for <b>5 minutes</b>.</p>

                    <p>If you didn't request this, please ignore this email.</p>"
            });

            return true;
        }
    }
}