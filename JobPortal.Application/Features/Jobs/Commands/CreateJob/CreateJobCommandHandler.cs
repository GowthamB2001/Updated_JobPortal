using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobHandler
           : IRequestHandler<CreateJobCommand, Guid>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IJobSkillRepository _jobSkillRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateJobHandler(
            IJobRepository jobRepository,
            IJobSkillRepository jobSkillRepository,
            IRecruiterRepository recruiterRepository,
            IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _jobSkillRepository = jobSkillRepository;
            _recruiterRepository = recruiterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateJobCommand request,
            CancellationToken cancellationToken)
        {
            var recruiter =
                await _recruiterRepository.GetByIdAsync(request.RecruiterId);

            if (recruiter == null)
                throw new Exception("Recruiter not found.");

            // If Company or CompanyOverview is empty, fallback to Recruiter's organization
            var company = string.IsNullOrWhiteSpace(request.Company)
                ? recruiter.OrganizationName
                : request.Company;

            var companyOverview = string.IsNullOrWhiteSpace(request.CompanyOverview)
                ? $"Leading organization in {recruiter.OrganizationType}"
                : request.CompanyOverview;

            // Formulate salary range string if not given
            var salaryRange = request.SalaryRange;
            if (string.IsNullOrWhiteSpace(salaryRange))
            {
                if (request.MinSalary.HasValue && request.MaxSalary.HasValue)
                {
                    salaryRange = $"₹{request.MinSalary:N0} - ₹{request.MaxSalary:N0}";
                }
                else if (request.MinSalary.HasValue)
                {
                    salaryRange = $"₹{request.MinSalary:N0}+";
                }
                else
                {
                    salaryRange = "Not Disclosed";
                }
            }

            var repostDays = request.AutoRepostDays > 0 ? request.AutoRepostDays : 1;

            var job = new Job
            {
                RecruiterId = request.RecruiterId,
                Title = request.Title,
                Company = company,
                Location = request.Location,
                SalaryRange = salaryRange,
                MinSalary = request.MinSalary,
                MaxSalary = request.MaxSalary,
                Currency = string.IsNullOrWhiteSpace(request.Currency) ? "INR" : request.Currency,
                Experience = request.Experience,
                JobType = request.JobType,
                WorkMode = request.WorkMode,
                JobDescription = request.JobDescription,
                Openings = request.Openings,
                CompanyOverview = companyOverview,
                AutoRepostDays = repostDays,
                IsDraft = request.IsDraft,

                PostedOn = DateTime.UtcNow,

                ExpiryDate = request.IsDraft
                    ? DateTime.UtcNow.AddYears(1)
                    : DateTime.UtcNow.AddDays(repostDays),

                IsActive = !request.IsDraft,

                RepostCount = 0
            };

            await _jobRepository.AddAsync(job);
            await _unitOfWork.SaveChangesAsync();

            if (request.KeySkills != null && request.KeySkills.Count > 0)
            {
                foreach (var skill in request.KeySkills)
                {
                    if (!string.IsNullOrWhiteSpace(skill))
                    {
                        await _jobSkillRepository.AddAsync(
                            new JobSkill
                            {
                                JobId = job.Id,
                                SkillName = skill.Trim()
                            });
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }

            return job.Id;
        }
    }
}
