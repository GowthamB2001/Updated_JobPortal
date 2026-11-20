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

            var filtered = jobs.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                var role = request.Role.Trim().ToLower();
                filtered = filtered.Where(j => !string.IsNullOrWhiteSpace(j.Title) && (j.Title.ToLower().Contains(role) || role.Contains(j.Title.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(request.Experience))
            {
                var exp = request.Experience.Trim().ToLower();
                filtered = filtered.Where(j => !string.IsNullOrWhiteSpace(j.Experience) && (j.Experience.ToLower().Contains(exp) || exp.Contains(j.Experience.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(request.WorkMode))
            {
                var wm = request.WorkMode.Trim().ToLower();
                filtered = filtered.Where(j => !string.IsNullOrWhiteSpace(j.WorkMode) && j.WorkMode.ToLower().Contains(wm));
            }

            if (request.MinSalary.HasValue)
            {
                decimal minVal = request.MinSalary.Value <= 100 ? request.MinSalary.Value * 100000m : request.MinSalary.Value;
                filtered = filtered.Where(j => (j.MaxSalary ?? j.MinSalary ?? 0) >= minVal);
            }

            if (request.MaxSalary.HasValue && request.MaxSalary.Value < 50)
            {
                decimal maxVal = request.MaxSalary.Value <= 100 ? request.MaxSalary.Value * 100000m : request.MaxSalary.Value;
                filtered = filtered.Where(j => (j.MinSalary ?? 0) <= maxVal);
            }

            return filtered.Select(job => new JobSearchDto
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
