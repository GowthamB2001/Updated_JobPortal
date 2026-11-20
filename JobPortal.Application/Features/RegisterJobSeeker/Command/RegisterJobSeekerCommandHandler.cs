using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker;
using JobPortal.Domain.Domain.Entities;
using MediatR;

namespace JobPortal.Application.Features.RegisterJobSeeker.Command
{
    public class RegisterJobSeekerCommandHandler
        : IRequestHandler<RegisterJobSeekerCommand, RegisterJobSeekerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterJobSeekerCommandHandler(
            IUnitOfWork unitOfWork,
            IFileService fileService,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<RegisterJobSeekerResponse> Handle(
            RegisterJobSeekerCommand request,
            CancellationToken cancellationToken)
        {
            // Serialize Companies or use CompaniesWorkedJson
            string? companiesJson = request.CompaniesWorkedJson;
            if (request.Companies != null && request.Companies.Any())
            {
                companiesJson = System.Text.Json.JsonSerializer.Serialize(request.Companies);
            }

            // Fallbacks for backward compatibility
            var yearsOfExp = request.YearsOfExperience;
            if (!yearsOfExp.HasValue && (request.TotalExperienceYears.HasValue || request.TotalExperienceMonths.HasValue))
            {
                yearsOfExp = (request.TotalExperienceYears ?? 0) + Math.Round((request.TotalExperienceMonths ?? 0) / 12.0m, 2);
            }

            var lastMonthSal = request.LastMonthSalary;
            if (!lastMonthSal.HasValue && request.LastCompanySalary.HasValue)
            {
                lastMonthSal = Math.Round(request.LastCompanySalary.Value / 12.0m, 2);
            }

            var lastRoleDesig = request.LastRoleDesignation;
            if (string.IsNullOrWhiteSpace(lastRoleDesig) && request.Companies != null && request.Companies.Any())
            {
                lastRoleDesig = request.Companies.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.Designation))?.Designation;
            }

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
                profile.YearsOfExperience = yearsOfExp;
                profile.LastRoleDesignation = lastRoleDesig;
                profile.LastMonthSalary = lastMonthSal;
                profile.PreferredLocation = request.PreferredLocation;
                profile.JobRole = request.JobRole;
                profile.LookingFor = request.LookingFor;

                // Mobile UI Registration Fields
                profile.CurrentLocation = request.CurrentLocation;
                profile.EmploymentStatus = request.EmploymentStatus;
                profile.NoticePeriod = request.NoticePeriod;
                profile.TotalExperienceYears = request.TotalExperienceYears;
                profile.TotalExperienceMonths = request.TotalExperienceMonths;
                profile.TotalCompaniesWorked = request.TotalCompaniesWorked;
                profile.CompaniesWorkedJson = companiesJson;
                profile.LastCompanySalary = request.LastCompanySalary;

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

                var token = _jwtTokenGenerator.GenerateToken(profile);
                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(profile.Id, "JobSeeker");

                return new RegisterJobSeekerResponse
                {
                    JobSeekerId = profile.Id,
                    Token = token,
                    RefreshToken = refreshToken,
                    IsProfileCompleted = true
                };
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
                YearsOfExperience = yearsOfExp,
                LastRoleDesignation = lastRoleDesig,
                LastMonthSalary = lastMonthSal,
                PreferredLocation = request.PreferredLocation,
                JobRole = request.JobRole,
                LookingFor = request.LookingFor,
                ResumeUrl = resumeUrl,
                Email = request.Email,

                // Mobile UI Registration Fields
                CurrentLocation = request.CurrentLocation,
                EmploymentStatus = request.EmploymentStatus,
                NoticePeriod = request.NoticePeriod,
                TotalExperienceYears = request.TotalExperienceYears,
                TotalExperienceMonths = request.TotalExperienceMonths,
                TotalCompaniesWorked = request.TotalCompaniesWorked,
                CompaniesWorkedJson = companiesJson,
                LastCompanySalary = request.LastCompanySalary,

                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.JobSeekerProfile
                .AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(entity);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(entity.Id, "JobSeeker");

            return new RegisterJobSeekerResponse
            {
                JobSeekerId = entity.Id,
                Token = token,
                RefreshToken = refreshToken,
                IsProfileCompleted = true
            };
        }
    }
}