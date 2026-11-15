using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJob
{
    public class GetApplicationsByJobQuery
        : IRequest<List<JobApplicationDto>>
    {
        public Guid JobId { get; set; }
        public Guid? RecruiterId { get; set; }
    }
}
