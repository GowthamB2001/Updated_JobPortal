using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Commands.ApplyJob
{
    public class ApplyJobCommand : IRequest<Guid>
    {
        public Guid JobId { get; set; }

        public Guid JobSeekerId { get; set; }

        public string? CoverLetter { get; set; }
    }
}
