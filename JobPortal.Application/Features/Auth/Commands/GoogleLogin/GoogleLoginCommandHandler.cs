using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Auth;
using JobPortal.Domain.Domain.Entities;
using MediatR;

namespace JobPortal.Application.Features.Auth.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler
        : IRequestHandler<GoogleLoginCommand, GoogleLoginResponse>
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public GoogleLoginCommandHandler(
            IGoogleAuthService googleAuthService,
            IUnitOfWork unitOfWork,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _googleAuthService = googleAuthService;
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<GoogleLoginResponse> Handle(
            GoogleLoginCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Verify Google Token
            var googleUser = await _googleAuthService
                .VerifyGoogleTokenAsync(request.IdToken);

            // 2. Find Google Login
            var googleLogin = await _unitOfWork.GoogleLogin
                .GetByGoogleIdAsync(googleUser.GoogleId);

            // 3. First time Google Login
            if (googleLogin == null)
            {
                googleLogin = new Domain.Domain.Entities.GoogleLogin
                {
                    Id = Guid.NewGuid(),
                    GoogleId = googleUser.GoogleId,
                    Name = googleUser.Name,
                    Email = googleUser.Email,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _unitOfWork.GoogleLogin
                    .AddAsync(googleLogin);

                await _unitOfWork.SaveChangesAsync();
            }

            // 4. Find JobSeekerProfile using
            //    existing Google-linked JobSeekerProfileId
            JobSeekerProfile? profile = null;

            if (googleLogin.JobSeekerProfileId.HasValue)
            {
                profile = await _unitOfWork.JobSeekerProfile
                    .GetByIdAsync(
                        googleLogin.JobSeekerProfileId.Value);
            }

            // 5. If not linked, find by Google email
            if (profile == null)
            {
                profile = await _unitOfWork.JobSeekerProfile
                    .GetByEmailAsync(googleUser.Email);
            }

            // 6. Create JobSeekerProfile only once
            if (profile == null)
            {
                profile = new JobSeekerProfile
                {
                    Id = Guid.NewGuid(),

                    Name = googleUser.Name,
                    Email = googleUser.Email,

                    MobileNo = string.Empty,
                    Gender = string.Empty,
                    Education = string.Empty,
                    Language = string.Empty,
                    KeySkills = string.Empty,
                    WorkExperience = string.Empty,
                    PreferredLocation = string.Empty,
                    JobRole = string.Empty,

                    YearsOfExperience = null,
                    LastRoleDesignation = null,
                    LastMonthSalary = null,
                    LookingFor = null,
                    ResumeUrl = null,

                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _unitOfWork.JobSeekerProfile
                    .AddAsync(profile);

                // IMPORTANT:
                // Link Google account to this SAME profile
                googleLogin.JobSeekerProfileId = profile.Id;

                await _unitOfWork.GoogleLogin
                    .UpdateAsync(googleLogin);

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                // Existing profile.
                // Never create another profile.

                if (googleLogin.JobSeekerProfileId != profile.Id)
                {
                    googleLogin.JobSeekerProfileId = profile.Id;

                    await _unitOfWork.GoogleLogin
                        .UpdateAsync(googleLogin);

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            // 7. Check profile completion
            bool profileCompleted =
                !string.IsNullOrWhiteSpace(profile.MobileNo) &&
                !string.IsNullOrWhiteSpace(profile.Gender) &&
                !string.IsNullOrWhiteSpace(profile.Education) &&
                !string.IsNullOrWhiteSpace(profile.Language) &&
                !string.IsNullOrWhiteSpace(profile.KeySkills) &&
                !string.IsNullOrWhiteSpace(profile.WorkExperience) &&
                !string.IsNullOrWhiteSpace(profile.PreferredLocation) &&
                !string.IsNullOrWhiteSpace(profile.JobRole);

            // 8. Generate JobSeeker JWT
            var token = _jwtTokenGenerator
                .GenerateToken(profile);

            // 9. Return SAME JobSeekerId
            return new GoogleLoginResponse
            {
                GoogleLoginId = googleLogin.Id,

                JobSeekerId = profile.Id,

                Name = profile.Name,

                Email = profile.Email,

                Token = token,

                RefreshToken = _jwtTokenGenerator.GenerateRefreshToken(profile.Id, "JobSeeker"),

                IsProfileCompleted = profileCompleted
            };
        }
    }
}