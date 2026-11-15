using JobPortal.Application.DTOs.Interviews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Queries
{
    public class GetInterviewsByApplicationQuery
         : IRequest<List<InterviewDto>>
    {
        public Guid JobApplicationId { get; set; }
    }
}
