using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByRecruiter
{
    public class GetApplicationsByRecruiterQuery : IRequest<List<JobApplicationDto>>
    {
        public Guid RecruiterId { get; set; }

        public string? Status { get; set; }
    }
}
