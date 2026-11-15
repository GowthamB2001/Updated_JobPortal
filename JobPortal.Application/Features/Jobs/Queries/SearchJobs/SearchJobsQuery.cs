using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.SearchJobs
{
    public class SearchJobsQuery : IRequest<List<JobSearchDto>>
    {
        public string? Keyword { get; set; }

        public string? Location { get; set; }

        public JobType? JobType { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
