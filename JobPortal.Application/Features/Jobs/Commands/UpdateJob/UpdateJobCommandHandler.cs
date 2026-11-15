using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            job.Title = request.Title;
            job.Company = request.Company;
            job.Location = request.Location;
            job.SalaryRange = request.SalaryRange;
            job.Experience = request.Experience;
            job.JobType = request.JobType;
            job.WorkMode = request.WorkMode;
            job.JobDescription = request.JobDescription;
            job.Openings = request.Openings;
            job.CompanyOverview = request.CompanyOverview;

            await _jobRepository.UpdateAsync(job);

            // Remove old skills
            foreach (var skill in job.JobSkills.ToList())
            {
                await _jobSkillRepository.DeleteAsync(skill);
            }

            // Add new skills
            foreach (var skill in request.KeySkills)
            {
                await _jobSkillRepository.AddAsync(
                    new JobPortal.Domain.Domain.Entities.JobSkill
                    {
                        JobId = job.Id,
                        SkillName = skill
                    });
            }

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
