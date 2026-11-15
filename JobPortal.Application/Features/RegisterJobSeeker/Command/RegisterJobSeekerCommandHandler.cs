using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker;
using JobPortal.Domain.Domain.Entities;
using MediatR;

namespace JobPortal.Application.Features.RegisterJobSeeker.Command
{
    public class RegisterJobSeekerCommandHandler
        : IRequestHandler<RegisterJobSeekerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public RegisterJobSeekerCommandHandler(
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Guid> Handle(
            RegisterJobSeekerCommand request,
            CancellationToken cancellationToken)
        {
            // =====================================================
            // GOOGLE USER
            // JobSeekerId already created during Google Login
            // =====================================================

            if (request.JobSeekerId.HasValue)
            {
                var profile = await _unitOfWork.JobSeekerProfile
                    .GetByIdAsync(request.JobSeekerId.Value);

                if (profile == null)
                {
                    throw new Exception(
                        "Job seeker profile not found.");
                }

                // Check mobile number only against another profile
                var mobileExists = await _unitOfWork.JobSeekerProfile
                    .GetByMobileNoAsync(request.MobileNo);

                if (mobileExists != null &&
                    mobileExists.Id != profile.Id)
                {
                    throw new Exception(
                        "Mobile number already registered.");
                }

                // Upload new resume if provided
                if (request.Resume != null &&
                    request.Resume.Length > 0)
                {
                    profile.ResumeUrl =
                        await _fileService.UploadResumeAsync(
                            request.Resume);
                }

                // =================================================
                // UPDATE SAME GOOGLE-CREATED PROFILE
                // =================================================

                profile.Name = request.Name;
                profile.MobileNo = request.MobileNo;
                profile.Gender = request.Gender;
                profile.Education = request.Education;
                profile.Language = request.Language;
                profile.KeySkills = request.KeySkills;
                profile.WorkExperience = request.WorkExperience;
                profile.YearsOfExperience =
                    request.YearsOfExperience;
                profile.LastRoleDesignation =
                    request.LastRoleDesignation;
                profile.LastMonthSalary =
                    request.LastMonthSalary;
                profile.PreferredLocation =
                    request.PreferredLocation;
                profile.JobRole = request.JobRole;
                profile.LookingFor = request.LookingFor;

                // Don't create another ID
                // Don't change Google-created identity
                // Keep the same Email
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    profile.Email = request.Email;
                }

                profile.ModifiedOn = DateTime.UtcNow;

                await _unitOfWork.JobSeekerProfile
                    .UpdateAsync(profile);

                await _unitOfWork.SaveChangesAsync();

                // SAME ID RETURN
                return profile.Id;
            }

            // =====================================================
            // NORMAL MOBILE REGISTRATION
            // No Google-created JobSeekerId
            // =====================================================

            var exists = await _unitOfWork.JobSeekerProfile
                .GetByMobileNoAsync(request.MobileNo);

            if (exists != null)
            {
                throw new Exception(
                    "Mobile number already registered.");
            }

            var resumeUrl =
                await _fileService.UploadResumeAsync(
                    request.Resume);

            // CREATE NEW PROFILE ONLY FOR NORMAL REGISTRATION
            var entity = new JobSeekerProfile
            {
                Id = Guid.NewGuid(),

                Name = request.Name,
                MobileNo = request.MobileNo,
                Gender = request.Gender,
                Education = request.Education,
                Language = request.Language,
                KeySkills = request.KeySkills,
                WorkExperience = request.WorkExperience,
                YearsOfExperience =
                    request.YearsOfExperience,
                LastRoleDesignation =
                    request.LastRoleDesignation,
                LastMonthSalary =
                    request.LastMonthSalary,
                PreferredLocation =
                    request.PreferredLocation,
                JobRole = request.JobRole,
                LookingFor = request.LookingFor,
                ResumeUrl = resumeUrl,
                Email = request.Email,

                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.JobSeekerProfile
                .AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }
    }
}