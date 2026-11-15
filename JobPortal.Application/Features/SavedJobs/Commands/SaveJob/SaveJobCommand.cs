using JobPortal.Application.DTOs.SavedJobs;
using MediatR;
using System;

namespace JobPortal.Application.Features.SavedJobs.Commands.SaveJob
{
    public class SaveJobCommand : IRequest<SavedJobDto>
    {
        public Guid JobSeekerProfileId { get; set; }
        public Guid JobId { get; set; }
    }
}
