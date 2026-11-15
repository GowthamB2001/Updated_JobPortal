using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJob
{
    public class GetApplicationsByJobQueryHandler
         : IRequestHandler<GetApplicationsByJobQuery, List<JobApplicationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetApplicationsByJobQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<JobApplicationDto>> Handle(
            GetApplicationsByJobQuery request,
            CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId);
            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID '{request.JobId}' was not found.");
            }

            // Verify the job belongs to this recruiter if recruiterId is provided
            if (request.RecruiterId.HasValue && job.RecruiterId != Guid.Empty && job.RecruiterId != request.RecruiterId.Value)
            {
                throw new UnauthorizedAccessException("You are not authorized to view applicants for this job.");
            }

            var applications = await _unitOfWork.JobApplications.GetApplicationsByJobAsync(request.JobId);

            return applications.Select(x => new JobApplicationDto
            {
                Id = x.Id,
                JobId = x.JobId,
                JobTitle = job.Title,
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
            }).ToList();
        }
    }
}
