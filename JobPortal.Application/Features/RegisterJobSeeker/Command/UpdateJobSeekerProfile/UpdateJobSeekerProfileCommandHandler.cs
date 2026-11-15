using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using MediatR;

namespace JobPortal.Application.Features.RegisterJobSeeker.Command.UpdateJobSeekerProfile
{
    public class UpdateJobSeekerProfileCommandHandler
        : IRequestHandler<UpdateJobSeekerProfileCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateJobSeekerProfileCommandHandler(
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task Handle(
            UpdateJobSeekerProfileCommand request,
            CancellationToken cancellationToken)
        {
            var profile = await _unitOfWork.JobSeekerProfile.GetByIdAsync(request.Id);

            if (profile == null)
                throw new Exception("Job Seeker not found.");

            profile.Name = request.Name;
            profile.Gender = request.Gender;
            profile.Education = request.Education;
            profile.Language = request.Language;
            profile.KeySkills = request.KeySkills;
            profile.WorkExperience = request.WorkExperience;
            profile.YearsOfExperience = request.YearsOfExperience;
            profile.LastRoleDesignation = request.LastRoleDesignation;
            profile.LastMonthSalary = request.LastMonthSalary;
            profile.PreferredLocation = request.PreferredLocation;
            profile.JobRole = request.JobRole;
            profile.LookingFor = request.LookingFor;


            if (request.Resume != null && request.Resume.Length > 0)
            {
                profile.ResumeUrl = await _fileService.UploadResumeAsync(request.Resume);
            }

            await _unitOfWork.JobSeekerProfile.UpdateAsync(profile);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}