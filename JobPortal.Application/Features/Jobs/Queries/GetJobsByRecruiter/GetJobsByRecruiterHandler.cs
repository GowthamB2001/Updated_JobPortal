using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter
{
    public class GetJobsByRecruiterHandler
        : IRequestHandler<GetJobsByRecruiterQuery, List<JobDto>>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobsByRecruiterHandler(
            IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<JobDto>> Handle(
            GetJobsByRecruiterQuery request,
            CancellationToken cancellationToken)
        {
            var jobs = await _jobRepository
                .GetJobsByRecruiterAsync(request.RecruiterId);

            return jobs.Select(job => new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Company = job.Company,
                Location = job.Location,
                SalaryRange = job.SalaryRange,
                Experience = job.Experience,
                JobType = job.JobType,
                WorkMode = job.WorkMode,
                PostedOn = job.PostedOn,
                ExpiryDate = job.ExpiryDate,
                RepostCount = job.RepostCount,
                IsActive = job.IsActive,

                Status =
                    job.IsActive &&
                    job.ExpiryDate > DateTime.UtcNow
                        ? "Active"
                        : "Expired",

                Skills = job.JobSkills
                    .Select(x => x.SkillName)
                    .ToList()

            }).ToList();
        }

    }
}