using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Models;
using JobPortal.Domain.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobPortal.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        // ===================== Recruiter JWT =====================
        public string GenerateToken(Recruiter recruiter)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, recruiter.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, recruiter.OfficialEmail),
                new Claim(ClaimTypes.NameIdentifier, recruiter.Id.ToString()),
                new Claim(ClaimTypes.Name, recruiter.RecruiterName),
                new Claim(ClaimTypes.Role, "Recruiter"),
                new Claim("OrganizationName", recruiter.OrganizationName),
                new Claim("OrganizationType", recruiter.OrganizationType.ToString())
            };

            return GenerateJwt(claims);
        }

        // ===================== Job Seeker JWT =====================
        public string GenerateToken(JobSeekerProfile jobSeeker)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jobSeeker.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, jobSeeker.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, jobSeeker.Id.ToString()),
                new Claim(ClaimTypes.Name, jobSeeker.Name),
                new Claim(ClaimTypes.Role, "JobSeeker")
            };

            return GenerateJwt(claims);
        }

        // ===================== Google Login JWT =====================
        public string GenerateGoogleToken(GoogleLogin googleLogin)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, googleLogin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, googleLogin.Email),
                new Claim(ClaimTypes.NameIdentifier, googleLogin.Id.ToString()),
                new Claim(ClaimTypes.Name, googleLogin.Name),
                new Claim(ClaimTypes.Role, "JobSeeker"),
                new Claim("GoogleId", googleLogin.GoogleId)
            };

            return GenerateJwt(claims);
        }

        // ===================== Common JWT Generator =====================
        private string GenerateJwt(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}