using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByRecruiter
{
    public class GetApplicationsByRecruiterQueryHandler
         : IRequestHandler<GetApplicationsByRecruiterQuery, List<JobApplicationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetApplicationsByRecruiterQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<JobApplicationDto>> Handle(
            GetApplicationsByRecruiterQuery request,
            CancellationToken cancellationToken)
        {
            var recruiterJobs = (await _unitOfWork.Jobs.GetJobsByRecruiterAsync(request.RecruiterId)).ToList();
            if (recruiterJobs.Count == 0)
            {
                return new List<JobApplicationDto>();
            }

            var jobDict = recruiterJobs.ToDictionary(j => j.Id);
            var jobIds = jobDict.Keys.ToHashSet();

            var allApplications = await _unitOfWork.JobApplications.GetAllAsync();
            var recruiterApps = allApplications.Where(a => jobIds.Contains(a.JobId)).ToList();

            if (!string.IsNullOrWhiteSpace(request.Status) && !request.Status.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                recruiterApps = recruiterApps
                    .Where(a => a.Status.ToString().Equals(request.Status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return recruiterApps.Select(x =>
            {
                jobDict.TryGetValue(x.JobId, out var job);

                return new JobApplicationDto
                {
                    Id = x.Id,
                    JobId = x.JobId,
                    JobTitle = job?.Title ?? x.JobRole,
                    Company = job?.Company ?? string.Empty,
                    JobLocation = job?.Location ?? x.PreferredLocation,
                    JobSeekerId = x.JobSeekerId,
                    Name = x.Name,
                    MobileNo = x.MobileNo,
                    Email = x.Email,
                    Gender = x.Gender,
                    Education = x.Education,
                    Language = x.Language,
                    KeySkills = x.KeySkills,
                    WorkExperience = x.WorkExperience,
                    YearsOfExperience = x.YearsOfExperience,
                    PreferredLocation = x.PreferredLocation,
                    JobRole = x.JobRole,
                    ResumeUrl = x.ResumeUrl,
                    CoverLetter = x.CoverLetter,
                    Status = x.Status,
                    AppliedDate = x.AppliedOn
                };
            }).OrderByDescending(x => x.AppliedDate).ToList();
        }
    }
}
