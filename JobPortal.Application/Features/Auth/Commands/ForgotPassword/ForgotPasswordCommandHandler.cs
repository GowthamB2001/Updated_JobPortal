using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Models.Email;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler
        : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IEmailVerificationRepository emailVerificationRepository,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailVerificationRepository = emailVerificationRepository;
            _emailService = emailService;
        }

        public async Task<ForgotPasswordResponse> Handle(
            ForgotPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var recruiter = await _unitOfWork.Recruiters
                .GetByOfficialEmailAsync(request.OfficialEmail);

            if (recruiter == null)
            {
                throw new Exception("Recruiter with this official email was not found.");
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();

            var verification = await _emailVerificationRepository
                .GetByCompanyEmailAsync(request.OfficialEmail);

            if (verification == null)
            {
                verification = new EmailVerification
                {
                    CompanyEmail = request.OfficialEmail,
                    Otp = otp,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    IsVerified = false
                };

                await _emailVerificationRepository.AddAsync(verification);
            }
            else
            {
                verification.Otp = otp;
                verification.CreatedAt = DateTime.UtcNow;
                verification.ExpiresAt = DateTime.UtcNow.AddMinutes(10);
                verification.IsVerified = false;

                await _emailVerificationRepository.UpdateAsync(verification);
            }

            await _unitOfWork.SaveChangesAsync();

            try
            {
                await _emailService.SendEmailAsync(new EmailMessage
                {
                    To = request.OfficialEmail,
                    Subject = "Interview Spot - Password Reset OTP",
                    Body = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px;'>
                            <h2 style='color: #2b6cb0;'>Interview Spot - Password Reset</h2>
                            <p>Hello <b>{recruiter.RecruiterName}</b>,</p>
                            <p>We received a request to reset the password for your recruiter account.</p>
                            <p>Your one-time password (OTP) is:</p>
                            <div style='background-color: #f7fafc; padding: 15px; text-align: center; border-radius: 6px; margin: 20px 0;'>
                                <span style='font-size: 32px; font-weight: bold; letter-spacing: 6px; color: #2d3748;'>{otp}</span>
                            </div>
                            <p>This OTP is valid for <b>10 minutes</b>. Do not share this code with anyone.</p>
                            <p>If you did not request a password reset, you can safely ignore this email.</p>
                            <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0;' />
                            <small style='color: #a0aec0;'>Interview Spot Recruiter Portal</small>
                        </div>"
                });
            }
            catch (Exception ex)
            {
                return new ForgotPasswordResponse
                {
                    Success = true,
                    Message = $"Password reset OTP generated. (Email delivery notice: {ex.Message})"
                };
            }

            return new ForgotPasswordResponse
            {
                Success = true,
                Message = "Password reset OTP has been sent to your registered official email."
            };
        }
    }
}
