using JobPortal.Application.DTOs.Jobs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetAllJobs
{
    public class GetAllJobsQuery
       : IRequest<List<JobListDto>>
    {
    }
}
