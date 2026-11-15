using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.SavedJobs;
using JobPortal.Application.Features.Jobs.Common;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.SavedJobs.Commands.SaveJob
{
    public class SaveJobCommandHandler : IRequestHandler<SaveJobCommand, SavedJobDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SavedJobDto> Handle(SaveJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId);
            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID '{request.JobId}' not found.");
            }

            var allSkills = await _unitOfWork.JobSkills.GetAllAsync();
            job.JobSkills = allSkills.Where(s => s.JobId == job.Id).ToList();

            var profile = await _unitOfWork.JobSeekerProfile.GetByIdAsync(request.JobSeekerProfileId);
            var savedJob = await _unitOfWork.SavedJobs.SaveJobAsync(request.JobSeekerProfileId, request.JobId);

            var applications = (await _unitOfWork.JobApplications.GetAllAsync()).ToList();
            var savedJobIds = await _unitOfWork.SavedJobs.GetSavedJobIdsAsync(request.JobSeekerProfileId);

            var jobDto = JobMappingHelper.MapToJobListDto(job, profile, applications, savedJobIds);
            jobDto.IsBookmarked = true;

            // Add notification for Job Seeker
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = request.JobSeekerProfileId,
                Title = "Job Saved",
                Message = $"You saved the job '{job.Title}' at {job.Company}.",
                Type = "SavedJob",
                TargetUrl = $"/jobs/{job.Id}",
                IsRead = false,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = false
            };
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            return new SavedJobDto
            {
                SavedJobId = savedJob.Id,
                JobSeekerProfileId = request.JobSeekerProfileId,
                JobId = job.Id,
                SavedOn = savedJob.CreatedOn,
                Job = jobDto
            };
        }
    }
}
