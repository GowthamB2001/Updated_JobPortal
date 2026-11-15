using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.SavedJobs;
using JobPortal.Application.Features.Jobs.Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.SavedJobs.Queries.GetSavedJobs
{
    public class GetSavedJobsQueryHandler : IRequestHandler<GetSavedJobsQuery, List<SavedJobDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSavedJobsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SavedJobDto>> Handle(GetSavedJobsQuery request, CancellationToken cancellationToken)
        {
            var savedJobs = await _unitOfWork.SavedJobs.GetByJobSeekerProfileIdAsync(request.JobSeekerProfileId);
            var profile = await _unitOfWork.JobSeekerProfile.GetByIdAsync(request.JobSeekerProfileId);
            var applications = (await _unitOfWork.JobApplications.GetAllAsync()).ToList();
            var savedJobIds = await _unitOfWork.SavedJobs.GetSavedJobIdsAsync(request.JobSeekerProfileId);

            var result = new List<SavedJobDto>();
            foreach (var item in savedJobs)
            {
                if (item.Job == null) continue;

                var jobDto = JobMappingHelper.MapToJobListDto(item.Job, profile, applications, savedJobIds);
                jobDto.IsBookmarked = true;

                result.Add(new SavedJobDto
                {
                    SavedJobId = item.Id,
                    JobSeekerProfileId = item.JobSeekerProfileId,
                    JobId = item.JobId,
                    SavedOn = item.CreatedOn,
                    Job = jobDto
                });
            }

            return result;
        }
    }
}
