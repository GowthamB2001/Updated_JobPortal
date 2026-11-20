using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;

namespace JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker
{
    public class RegisterJobSeekerResponse
    {
        public Guid JobSeekerId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool IsProfileCompleted { get; set; } = true;
    }

    public class RegisterJobSeekerCommand
        : JobSeekerRegistrationRequest, IRequest<RegisterJobSeekerResponse>
    {
        // Google login-la already create aana ID
        public Guid? JobSeekerId { get; set; }
    }
}
