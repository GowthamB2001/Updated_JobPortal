using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Commands.UpdateInterviewStatus
{
    public class UpdateInterviewStatusCommand
        : IRequest<bool>
    {
        public Guid InterviewId { get; set; }

        public InterviewStatus Status { get; set; }
    }
}
