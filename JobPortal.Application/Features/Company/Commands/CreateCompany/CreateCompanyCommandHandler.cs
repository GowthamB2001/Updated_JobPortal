using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;

namespace JobPortal.Application.Features.Company.Commands.CreateCompany
{
    public class CreateCompanyCommandHandler
        : IRequestHandler<CreateCompanyCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompanyCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateCompanyCommand request,
            CancellationToken cancellationToken)
        {
            // Check if recruiter already has a company
            var recruiterCompany = await _unitOfWork.Companies
                .GetByRecruiterIdAsync(request.RecruiterId);

            if (recruiterCompany != null)
            {
                throw new Exception("Company profile already exists.");
            }

            // Check duplicate official email
            var existingCompany = await _unitOfWork.Companies
                .GetByOfficialEmailAsync(request.OfficialEmail);

            if (existingCompany != null)
            {
                throw new Exception("Official email already exists.");
            }

            var company = new Domain.Domain.Entities.Company
            {
                CompanyName = request.CompanyName,
                Website = request.Website,
                Address = request.Address,
                OfficialEmail = request.OfficialEmail,
                RecruiterId = request.RecruiterId,
                IsEmailVerified = false,
                EmailOtp = null,
                OtpExpiry = null
            };

            await _unitOfWork.Companies.AddAsync(company);

            await _unitOfWork.SaveChangesAsync();

            return company.Id;
        }
    }
}