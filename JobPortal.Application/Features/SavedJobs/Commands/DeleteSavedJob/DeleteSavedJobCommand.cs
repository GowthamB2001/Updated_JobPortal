using MediatR;
using System;

namespace JobPortal.Application.Features.SavedJobs.Commands.DeleteSavedJob
{
    public class DeleteSavedJobCommand : IRequest<bool>
    {
        public Guid JobSeekerProfileId { get; set; }
        public Guid JobId { get; set; }
    }
}
