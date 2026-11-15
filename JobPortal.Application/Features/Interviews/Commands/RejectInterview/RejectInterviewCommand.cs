using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Commands.RejectInterview
{
    public class RejectInterviewCommand
       : IRequest<bool>
    {
        public Guid InterviewId { get; set; }
    }
}
