using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Commands.CreateInterview
{
    public class CreateInterviewCommand
         : IRequest<Guid>
    {
        public Guid JobApplicationId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string MeetingLink { get; set; }

        public string Remarks { get; set; }
    }
}
