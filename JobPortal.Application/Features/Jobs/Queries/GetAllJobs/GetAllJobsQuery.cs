using JobPortal.Application.DTOs.Jobs;
using MediatR;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.Jobs.Queries.GetAllJobs
{
    public class GetAllJobsQuery : IRequest<List<JobListDto>>
    {
        public Guid? JobSeekerId { get; set; }
    }
}
