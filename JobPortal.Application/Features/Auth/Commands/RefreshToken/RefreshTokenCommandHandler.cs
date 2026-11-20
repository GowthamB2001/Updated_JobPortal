using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Auth;
using MediatR;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<RefreshTokenResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                throw new Exception("RefreshToken is required.");
            }

            // 1. Validate refresh token and extract claims
            var principal = _jwtTokenGenerator.GetPrincipalFromToken(request.RefreshToken, validateLifetime: true);

            // 2. Fallback: if client also provided expired access token
            if (principal == null && !string.IsNullOrWhiteSpace(request.Token))
            {
                principal = _jwtTokenGenerator.GetPrincipalFromToken(request.Token, validateLifetime: false);
            }

            if (principal == null)
            {
                throw new Exception("Invalid or expired refresh token. Please log in again.");
            }

            // 3. Extract UserId and Role
            var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? principal.FindFirst("sub")?.Value;

            var role = principal.FindFirst(ClaimTypes.Role)?.Value
                ?? principal.FindFirst("role")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                throw new Exception("Invalid token claims: User identifier could not be determined.");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new Exception("Invalid token claims: Role could not be determined.");
            }

            // 4. Generate new tokens based on user role
            if (role.Equals("Recruiter", StringComparison.OrdinalIgnoreCase))
            {
                var recruiter = await _unitOfWork.Recruiters.GetByIdAsync(userId);
                if (recruiter == null || recruiter.IsDeleted)
                {
                    throw new Exception("Recruiter account not found or deactivated.");
                }

                var newAccessToken = _jwtTokenGenerator.GenerateToken(recruiter);
                var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(recruiter.Id, "Recruiter");

                return new RefreshTokenResponse
                {
                    Success = true,
                    Message = "Token refreshed successfully.",
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                    UserId = recruiter.Id,
                    Role = "Recruiter"
                };
            }
            else if (role.Equals("JobSeeker", StringComparison.OrdinalIgnoreCase))
            {
                var profile = await _unitOfWork.JobSeekerProfile.GetByIdAsync(userId);
                if (profile == null || profile.IsDeleted)
                {
                    throw new Exception("Job seeker account not found or deactivated.");
                }

                var newAccessToken = _jwtTokenGenerator.GenerateToken(profile);
                var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(profile.Id, "JobSeeker");

                return new RefreshTokenResponse
                {
                    Success = true,
                    Message = "Token refreshed successfully.",
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                    UserId = profile.Id,
                    Role = "JobSeeker"
                };
            }
            else
            {
                throw new Exception($"Unsupported user role '{role}' for token refresh.");
            }
        }
    }
}
