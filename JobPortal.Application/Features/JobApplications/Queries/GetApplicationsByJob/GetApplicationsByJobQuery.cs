using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJob
{
    public class GetApplicationsByJobQuery
        : IRequest<List<JobApplicationDto>>
    {
        public Guid JobId { get; set; }
    }
}
