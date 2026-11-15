using JobPortal.Application.DTOs.Common;
using JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetPagedJobs
{
    public class GetPagedJobsQuery
     : IRequest<PagedResult<JobDto>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
