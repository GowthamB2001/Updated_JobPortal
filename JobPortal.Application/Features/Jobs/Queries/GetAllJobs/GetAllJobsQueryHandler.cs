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

            return jobs.Select(job => JobMappingHelper.MapToJobListDto(job, profile, applications, savedJobIds)).ToList();
        }
    }
}
