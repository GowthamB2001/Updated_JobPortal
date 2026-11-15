using JobPortal.Application.DTOs.JobSeekers;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker
{
    public class RegisterJobSeekerCommand
        : JobSeekerRegistrationRequest, IRequest<Guid>
    {
        // Google login-la already create aana ID
        public Guid? JobSeekerId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string KeySkills { get; set; } = string.Empty;
        public string WorkExperience { get; set; } = string.Empty;
        public decimal? YearsOfExperience { get; set; }
        public string? LastRoleDesignation { get; set; }
        public decimal? LastMonthSalary { get; set; }
        public string PreferredLocation { get; set; } = string.Empty;
        public string JobRole { get; set; } = string.Empty;
        public string? LookingFor { get; set; }
        public IFormFile? Resume { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}