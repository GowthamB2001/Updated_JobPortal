using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Jobs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetAllJobs
{
    public class GetAllJobsQueryHandler
            : IRequestHandler<GetAllJobsQuery, List<JobListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllJobsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<JobListDto>> Handle(
            GetAllJobsQuery request,
            CancellationToken cancellationToken)
        {
            var jobs = await _unitOfWork.Jobs.GetAllJobsAsync();

            return jobs.Select(job => new JobListDto
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
                JobDescription = job.JobDescription,
                Openings = job.Openings,

                // Company entity-la add pannina fill pannunga
                CompanyProfile = null,
                CompanyOverview = null,

                KeySkills = job.JobSkills
                    .Select(x => x.SkillName)
                    .ToList()
            }).ToList();
        }
    }
}
