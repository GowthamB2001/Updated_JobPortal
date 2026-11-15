using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Models.Email;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler
        : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IEmailService _emailService;

        public ResetPasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IEmailVerificationRepository emailVerificationRepository,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailVerificationRepository = emailVerificationRepository;
            _emailService = emailService;
        }

        public async Task<ResetPasswordResponse> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new Exception("New password and confirm password do not match.");
            }

            var recruiter = await _unitOfWork.Recruiters
                .GetByOfficialEmailAsync(request.OfficialEmail);

            if (recruiter == null)
            {
                throw new Exception("Recruiter with this official email was not found.");
            }

            var verification = await _emailVerificationRepository
                .GetByCompanyEmailAsync(request.OfficialEmail);

            if (verification == null)
            {
                throw new Exception("No password reset request found. Please request an OTP first.");
            }

            if (verification.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("OTP has expired. Please request a new password reset OTP.");
            }

            if (verification.Otp != request.Otp && !verification.IsVerified)
            {
                throw new Exception("Invalid OTP. Please check the code and try again.");
            }

            // Update recruiter's password hash using BCrypt
            recruiter.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            recruiter.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.Recruiters.UpdateAsync(recruiter);

            // Invalidate the OTP record so it cannot be reused
            verification.Otp = string.Empty;
            verification.IsVerified = false;
            await _emailVerificationRepository.UpdateAsync(verification);

            await _unitOfWork.SaveChangesAsync();

            // Send confirmation email
            try
            {
                await _emailService.SendEmailAsync(new EmailMessage
                {
                    To = recruiter.OfficialEmail,
                    Subject = "Interview Spot - Password Changed Successfully",
                    Body = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px;'>
                            <h2 style='color: #38a169;'>Password Changed Successfully</h2>
                            <p>Hello <b>{recruiter.RecruiterName}</b>,</p>
                            <p>Your password for your Interview Spot Recruiter account has been updated successfully.</p>
                            <p>You can now sign in with your new password.</p>
                            <p>If you did not make this change, please contact our support team immediately.</p>
                            <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0;' />
                            <small style='color: #a0aec0;'>Interview Spot Recruiter Portal</small>
                        </div>"
                });
            }
            catch
            {
                // Non-critical; password has already been successfully updated
            }

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "Password has been reset successfully. You can now log in with your new password."
            };
        }
    }
}
