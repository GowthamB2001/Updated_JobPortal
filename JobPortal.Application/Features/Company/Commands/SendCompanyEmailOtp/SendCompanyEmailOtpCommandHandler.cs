using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Company.Commands.SendCompanyEmailOtp
{
    public class SendCompanyEmailOtpCommandHandler
        : IRequestHandler<SendCompanyEmailOtpCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public SendCompanyEmailOtpCommandHandler(
            IUnitOfWork unitOfWork,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(
            SendCompanyEmailOtpCommand request,
            CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies
                .GetByRecruiterIdAsync(request.RecruiterId);

            if (company == null)
                throw new Exception("Company not found.");

            if (company.IsEmailVerified)
                throw new Exception("Email already verified.");

            var otp = Random.Shared.Next(100000, 999999).ToString();

            company.EmailOtp = otp;
            company.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

            await _unitOfWork.Companies.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync();

            var emailMessage = new EmailMessage
            {
                To = company.OfficialEmail,
                Subject = "Email Verification OTP",
                Body = $@"
        <h2>Job Portal</h2>
        <p>Your OTP is:</p>
        <h1>{otp}</h1>
        <p>This OTP is valid for 10 minutes.</p>"
            };

            await _emailService.SendEmailAsync(emailMessage);
            return Unit.Value;
        }
    }
}
