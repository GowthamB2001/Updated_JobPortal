using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Jobs;
using JobPortal.Application.Features.Jobs.Common;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            JobSeekerProfile? profile = null;
            IReadOnlyCollection<Guid>? savedJobIds = null;
            if (request.JobSeekerId.HasValue)
            {
                profile = await _unitOfWork.JobSeekerProfile.GetByIdAsync(request.JobSeekerId.Value);
                savedJobIds = await _unitOfWork.SavedJobs.GetSavedJobIdsAsync(request.JobSeekerId.Value);
            }

            var jobs = await _unitOfWork.Jobs.GetAllJobsAsync();
            var applications = (await _unitOfWork.JobApplications.GetAllAsync()).ToList();

            var filteredJobs = jobs.Where(j => j.IsActive && !j.IsDeleted && j.ExpiryDate > DateTime.UtcNow).AsEnumerable();

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var kw = request.Keyword.Trim().ToLower();
                filteredJobs = filteredJobs.Where(j =>
                    (!string.IsNullOrWhiteSpace(j.Title) && j.Title.ToLower().Contains(kw)) ||
                    (!string.IsNullOrWhiteSpace(j.Company) && j.Company.ToLower().Contains(kw)) ||
                    (j.JobSkills != null && j.JobSkills.Any(s => !string.IsNullOrWhiteSpace(s.SkillName) && s.SkillName.ToLower().Contains(kw))));
            }

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                var role = request.Role.Trim().ToLower();
                filteredJobs = filteredJobs.Where(j =>
                    (!string.IsNullOrWhiteSpace(j.Title) && (j.Title.ToLower().Contains(role) || role.Contains(j.Title.ToLower()))));
            }

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                var loc = request.Location.Trim().ToLower();
                filteredJobs = filteredJobs.Where(j =>
                    !string.IsNullOrWhiteSpace(j.Location) && j.Location.ToLower().Contains(loc));
            }

            if (!string.IsNullOrWhiteSpace(request.Experience))
            {
                var exp = request.Experience.Trim().ToLower();
                filteredJobs = filteredJobs.Where(j =>
                    !string.IsNullOrWhiteSpace(j.Experience) && (j.Experience.ToLower().Contains(exp) || exp.Contains(j.Experience.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(request.WorkMode))
            {
                var wm = request.WorkMode.Trim().ToLower();
                filteredJobs = filteredJobs.Where(j =>
                    !string.IsNullOrWhiteSpace(j.WorkMode) && j.WorkMode.ToLower().Contains(wm));
            }

            if (!string.IsNullOrWhiteSpace(request.JobType))
            {
                var jt = request.JobType.Trim().Replace(" ", "").ToLower();
                filteredJobs = filteredJobs.Where(j =>
                    j.JobType.ToString().ToLower() == jt);
            }

            if (request.MinSalary.HasValue)
            {
                // If value <= 100, treat as LPA (e.g. 5 LPA = 500,000 INR)
                decimal minVal = request.MinSalary.Value <= 100 ? request.MinSalary.Value * 100000m : request.MinSalary.Value;
                filteredJobs = filteredJobs.Where(j => (j.MaxSalary ?? j.MinSalary ?? 0) >= minVal);
            }

            if (request.MaxSalary.HasValue && request.MaxSalary.Value < 50)
            {
                decimal maxVal = request.MaxSalary.Value <= 100 ? request.MaxSalary.Value * 100000m : request.MaxSalary.Value;
                filteredJobs = filteredJobs.Where(j => (j.MinSalary ?? 0) <= maxVal);
            }

            return filteredJobs
                .OrderByDescending(j => j.PostedOn)
                .Select(job => JobMappingHelper.MapToJobListDto(job, profile, applications, savedJobIds))
                .ToList();
        }
    }
}
