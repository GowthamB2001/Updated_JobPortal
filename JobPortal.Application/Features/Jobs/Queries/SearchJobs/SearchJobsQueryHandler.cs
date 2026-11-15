using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.SearchJobs
{
    public class SearchJobsHandler
            : IRequestHandler<SearchJobsQuery, List<JobSearchDto>>
    {
        private readonly IJobRepository _jobRepository;

        public SearchJobsHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<JobSearchDto>> Handle(
            SearchJobsQuery request,
            CancellationToken cancellationToken)
        {
            var jobs = await _jobRepository.SearchJobsAsync(
                request.Keyword,
                request.Location,
                request.JobType,
                request.PageNumber,
                request.PageSize);

            return jobs.Select(job => new JobSearchDto
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

                Skills = job.JobSkills
                    .Select(x => x.SkillName)
                    .ToList()

            }).ToList();
        }
    }
}
