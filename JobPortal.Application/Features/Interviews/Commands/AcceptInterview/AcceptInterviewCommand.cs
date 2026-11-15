using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Commands.AcceptInterview
{
    public class AcceptInterviewCommand
     : IRequest<bool>
    {
        public Guid InterviewId { get; set; }
    }
}
