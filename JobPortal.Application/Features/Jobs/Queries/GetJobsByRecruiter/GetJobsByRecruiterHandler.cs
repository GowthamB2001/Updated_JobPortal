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
        private readonly IUnitOfWork _unitOfWork;

        public GetJobsByRecruiterHandler(
            IJobRepository jobRepository,
            IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<JobDto>> Handle(
            GetJobsByRecruiterQuery request,
            CancellationToken cancellationToken)
        {
            var jobs = await _jobRepository
                .GetJobsByRecruiterAsync(request.RecruiterId);

            // Fetch all applications in real-time to compute applicant counts per job
            var allApplications = await _unitOfWork.JobApplications.GetAllAsync();
            var appCountMap = allApplications
                .GroupBy(a => a.JobId)
                .ToDictionary(g => g.Key, g => g.Count());

            return jobs.Select(job =>
            {
                int applicantCount = appCountMap.TryGetValue(job.Id, out var count) ? count : 0;

                return new JobDto
                {
                    Id = job.Id,
                    RecruiterId = job.RecruiterId,
                    RecruiterType = job.Recruiter?.OrganizationType?.ToString(),
                    Title = job.Title,
                    Company = job.Company,
                    Location = job.Location,
                    SalaryRange = job.SalaryRange,
                    MinSalary = job.MinSalary,
                    MaxSalary = job.MaxSalary,
                    Currency = !string.IsNullOrWhiteSpace(job.Currency) ? job.Currency : "INR",
                    Experience = job.Experience,
                    JobType = job.JobType,
                    WorkMode = job.WorkMode,
                    JobDescription = job.JobDescription ?? string.Empty,
                    Openings = job.Openings ?? string.Empty,
                    CompanyOverview = job.CompanyOverview ?? string.Empty,
                    AutoRepostDays = job.AutoRepostDays,
                    IsDraft = job.IsDraft,
                    PostedOn = job.PostedOn,
                    ExpiryDate = job.ExpiryDate,
                    RepostCount = job.RepostCount,
                    IsActive = job.IsActive,

                    Status =
                        job.IsActive &&
                        job.ExpiryDate > DateTime.UtcNow
                            ? "Active"
                            : "Expired",

                    ApplicantCount = applicantCount,
                    AppliedCandidateCount = applicantCount,
                    AppliedCount = applicantCount,
                    ApplicantCountText = $"{applicantCount} {(applicantCount == 1 ? "Applicant" : "Applicants")}",

                    Skills = job.JobSkills
                        .Select(x => x.SkillName)
                        .ToList()
                };
            }).ToList();
        }
    }
}