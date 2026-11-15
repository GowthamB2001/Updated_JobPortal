using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.UpdateJob
{
    public class UpdateJobHandler
            : IRequestHandler<UpdateJobCommand, Unit>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IJobSkillRepository _jobSkillRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateJobHandler(
            IJobRepository jobRepository,
            IJobSkillRepository jobSkillRepository,
            IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _jobSkillRepository = jobSkillRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(
            UpdateJobCommand request,
            CancellationToken cancellationToken)
        {
            var job = await _jobRepository
                .GetJobByIdAndRecruiterAsync(
                    request.JobId,
                    request.RecruiterId);

            if (job == null)
                throw new Exception("Job not found.");

            // Formulate salary range if needed
            var salaryRange = request.SalaryRange;
            if (string.IsNullOrWhiteSpace(salaryRange))
            {
                if (request.MinSalary.HasValue && request.MaxSalary.HasValue)
                    salaryRange = $"₹{request.MinSalary:N0} - ₹{request.MaxSalary:N0}";
                else if (request.MinSalary.HasValue)
                    salaryRange = $"₹{request.MinSalary:N0}+";
            }

            job.Title = request.Title;
            job.Company = request.Company;
            job.Location = request.Location;
            job.SalaryRange = salaryRange ?? job.SalaryRange;
            job.MinSalary = request.MinSalary ?? job.MinSalary;
            job.MaxSalary = request.MaxSalary ?? job.MaxSalary;
            job.Currency = string.IsNullOrWhiteSpace(request.Currency) ? job.Currency : request.Currency;
            job.Experience = request.Experience;
            job.JobType = request.JobType;
            job.WorkMode = request.WorkMode;
            job.JobDescription = request.JobDescription;
            job.Openings = request.Openings;
            job.CompanyOverview = request.CompanyOverview;
            job.AutoRepostDays = request.AutoRepostDays > 0 ? request.AutoRepostDays : job.AutoRepostDays;
            job.IsDraft = request.IsDraft;
            job.IsActive = !request.IsDraft;
            job.ModifiedOn = DateTime.UtcNow;

            await _jobRepository.UpdateAsync(job);

            // Remove old skills
            if (job.JobSkills != null)
            {
                foreach (var skill in job.JobSkills.ToList())
                {
                    await _jobSkillRepository.DeleteAsync(skill);
                }
            }

            // Add new skills
            if (request.KeySkills != null)
            {
                foreach (var skill in request.KeySkills)
                {
                    if (!string.IsNullOrWhiteSpace(skill))
                    {
                        await _jobSkillRepository.AddAsync(
                            new JobPortal.Domain.Domain.Entities.JobSkill
                            {
                                JobId = job.Id,
                                SkillName = skill.Trim()
                            });
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
