using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;

namespace JobPortal.Application.Features.Auth.Commands.RegisterRecruiter
{
    public class RegisterRecruiterCommandHandler
        : IRequestHandler<RegisterRecruiterCommand, bool>
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

        public async Task<bool> Handle(
            RegisterRecruiterCommand request,
            CancellationToken cancellationToken)
        {
            // Check Email Verification
            var verification = await _emailVerificationRepository
                .GetByCompanyEmailAsync(request.OfficialEmail);

            if (verification == null)
                throw new Exception("Email verification not found.");

            if (!verification.IsVerified)
                throw new Exception("Please verify your email first.");

            // Check Already Registered
            var exists = await _recruiterRepository
                .ExistsByOfficialEmailAsync(request.OfficialEmail);

            if (exists)
                throw new Exception("Recruiter already registered.");

            // Create Recruiter
            var recruiter = new Recruiter
            {
                OfficialEmail = request.OfficialEmail,
                OrganizationType = request.OrganizationType,
                RecruiterName = request.RecruiterName,
                OrganizationName = request.OrganizationName,
                Address = request.Address,
                MobileNumber = request.MobileNumber,
                EmployeeId = request.EmployeeId,
                GstNumber = request.GstNumber,
                Website = request.Website,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _recruiterRepository.AddAsync(recruiter);

            // Delete OTP Record
            await _emailVerificationRepository.DeleteAsync(verification);

            // IMPORTANT - Save all changes
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}