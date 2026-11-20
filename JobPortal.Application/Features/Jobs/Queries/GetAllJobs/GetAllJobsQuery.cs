using JobPortal.Application.DTOs.Jobs;
using MediatR;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.Jobs.Queries.GetAllJobs
{
    public class GetAllJobsQuery : IRequest<List<JobListDto>>
    {
        public Guid? JobSeekerId { get; set; }

        public string? Keyword { get; set; }

        public string? Role { get; set; }

        public string? Location { get; set; }

        public string? Experience { get; set; }

        public string? WorkMode { get; set; }

        public string? JobType { get; set; }

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }
    }
}
