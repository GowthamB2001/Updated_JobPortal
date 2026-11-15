using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommand
         : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }

        public ApplicationStatus Status { get; set; }

        public string? RecruiterRemarks { get; set; }
    }
}
