using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.RegisterRecruiter
{
    public class RegisterRecruiterCommandHandler
        : IRequestHandler<RegisterRecruiterCommand, Guid>
    {
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterRecruiterCommandHandler(
            IRecruiterRepository recruiterRepository,
            IEmailVerificationRepository emailVerificationRepository,
            IUnitOfWork unitOfWork)
        {
            _recruiterRepository = recruiterRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            RegisterRecruiterCommand request,
            CancellationToken cancellationToken)
        {
            var email = !string.IsNullOrWhiteSpace(request.OfficialEmail)
                ? request.OfficialEmail.Trim()
                : request.CompanyEmail?.Trim();

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Official company email is required.");

            var companyName = !string.IsNullOrWhiteSpace(request.CompanyName)
                ? request.CompanyName.Trim()
                : (!string.IsNullOrWhiteSpace(request.OrganizationName) ? request.OrganizationName.Trim() : "");

            if (string.IsNullOrWhiteSpace(companyName))
                throw new Exception("Company Name is required.");

            var recruiterName = request.RecruiterName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(recruiterName))
            {
                var parts = new[] { request.FirstName, request.LastName }
                    .Where(p => !string.IsNullOrWhiteSpace(p));
                recruiterName = string.Join(" ", parts).Trim();
            }

            if (string.IsNullOrWhiteSpace(recruiterName))
                throw new Exception("First Name or Recruiter Name is required.");

            var designation = !string.IsNullOrWhiteSpace(request.Designation)
                ? request.Designation.Trim()
                : request.JobTitle?.Trim();

            // 1. Check Email Verification
            var verification = await _emailVerificationRepository
                .GetByCompanyEmailAsync(email);

            if (verification == null)
                throw new Exception("Email verification record not found. Please send verification code first.");

            if (!verification.IsVerified)
                throw new Exception("Please verify your official company email with the OTP first.");

            // 2. Check Already Registered
            var exists = await _recruiterRepository
                .ExistsByOfficialEmailAsync(email);

            if (exists)
                throw new Exception("A recruiter with this official email is already registered.");

            // 3. Create Recruiter
            var recruiter = new Recruiter
            {
                Id = Guid.NewGuid(),
                OfficialEmail = email,
                OrganizationType = request.OrganizationType,
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                RecruiterName = recruiterName,
                Designation = designation,
                OrganizationName = companyName,
                Address = request.Address?.Trim() ?? string.Empty,
                MobileNumber = request.MobileNumber.Trim(),
                EmployeeId = request.EmployeeId?.Trim(),
                GstNumber = request.GstNumber?.Trim(),
                Website = request.Website?.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await _recruiterRepository.AddAsync(recruiter);

            // 4. Delete OTP Record
            await _emailVerificationRepository.DeleteAsync(verification);

            // 5. Save all changes
            await _unitOfWork.SaveChangesAsync();

            return recruiter.Id;
        }
    }
}
