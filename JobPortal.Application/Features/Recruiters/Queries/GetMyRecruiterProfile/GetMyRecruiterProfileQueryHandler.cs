using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Recruiters;
using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Queries.GetMyRecruiterProfile
{
    public class GetMyRecruiterProfileQueryHandler
        : IRequestHandler<GetMyRecruiterProfileQuery, RecruiterProfileDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyRecruiterProfileQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RecruiterProfileDto?> Handle(
            GetMyRecruiterProfileQuery request,
            CancellationToken cancellationToken)
        {
            var recruiter = await _unitOfWork.Recruiters
                .GetByIdAsync(request.RecruiterId);

            if (recruiter == null)
                return null;

            // Retrieve recruiter's posted jobs for stats and overview
            var allJobs = await _unitOfWork.Jobs.GetAllAsync();
            var recruiterJobs = allJobs
                .Where(j => j.RecruiterId == recruiter.Id)
                .ToList();

            var companyOverview = recruiterJobs
                .FirstOrDefault(j => !string.IsNullOrWhiteSpace(j.CompanyOverview))?.CompanyOverview;

            if (string.IsNullOrWhiteSpace(companyOverview))
            {
                companyOverview = $"{recruiter.OrganizationName} is a verified organization on Job Portal.";
            }

            string companyType = recruiter.OrganizationType switch
            {
                OrganizationType.OwnCompany => "In-house HR",
                OrganizationType.Consultancy => "Staffing Agency / Consultancy",
                _ => recruiter.OrganizationType.ToString()
            };

            int activeJobsCount = recruiterJobs.Count(j => j.IsActive && j.ExpiryDate > DateTime.UtcNow);

            return new RecruiterProfileDto
            {
                Id = recruiter.Id,
                FirstName = recruiter.FirstName,
                LastName = recruiter.LastName,
                Designation = recruiter.Designation,
                FullName = recruiter.RecruiterName,
                OfficialEmail = recruiter.OfficialEmail,
                CompanyName = recruiter.OrganizationName,
                CompanyType = companyType,
                OrganizationType = (int)recruiter.OrganizationType,
                CompanyLocation = recruiter.Address,
                EmployeeId = recruiter.EmployeeId ?? "EMP-001",
                PhoneNumber = recruiter.MobileNumber,
                AccountStatus = recruiter.IsDeleted ? "Inactive" : "Active",
                CompanyOverview = companyOverview,
                Website = recruiter.Website,
                GstNumber = recruiter.GstNumber,
                AvailableCredits = 10,
                TotalJobsPosted = recruiterJobs.Count,
                ActiveJobs = activeJobsCount,
                PrivacyPolicyUrl = "/legal/privacy-policy",
                TermsAndConditionsUrl = "/legal/terms-and-conditions"
            };
        }
    }
}
