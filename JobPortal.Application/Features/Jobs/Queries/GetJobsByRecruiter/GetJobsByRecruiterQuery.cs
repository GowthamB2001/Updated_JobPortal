using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter
{
    public class GetJobsByRecruiterQuery
        : IRequest<List<JobDto>>
    {
        public Guid RecruiterId { get; set; }
    }
}
