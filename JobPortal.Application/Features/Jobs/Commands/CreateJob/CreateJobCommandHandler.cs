using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var job = new Job
            {
                RecruiterId = request.RecruiterId,
                Title = request.Title,
                Company = request.Company,
                Location = request.Location,
                SalaryRange = request.SalaryRange,
                Experience = request.Experience,
                JobType = request.JobType,
                WorkMode = request.WorkMode,
                JobDescription = request.JobDescription,
                Openings = request.Openings,
                CompanyOverview = request.CompanyOverview,

                PostedOn = DateTime.UtcNow,

                // Automatically expires after 24 hours
                ExpiryDate = DateTime.UtcNow.AddHours(24),

                IsActive = true,

                RepostCount = 0
            };

            await _jobRepository.AddAsync(job);

            await _unitOfWork.SaveChangesAsync();

            foreach (var skill in request.KeySkills)
            {
                await _jobSkillRepository.AddAsync(
                    new JobSkill
                    {
                        JobId = job.Id,
                        SkillName = skill
                    });
            }

            await _unitOfWork.SaveChangesAsync();

            return job.Id;
        }
    }
}
