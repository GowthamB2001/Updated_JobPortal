using JobPortal.Application.DTOs.JobSeekers;
using MediatR;

namespace JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker
{
    public class RegisterJobSeekerCommand
        : JobSeekerRegistrationRequest, IRequest<Guid>
    {
        // Google login-la already create aana ID
        public Guid? JobSeekerId { get; set; }
    }
}
