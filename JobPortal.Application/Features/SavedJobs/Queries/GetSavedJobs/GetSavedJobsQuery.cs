using JobPortal.Application.DTOs.SavedJobs;
using MediatR;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.SavedJobs.Queries.GetSavedJobs
{
    public class GetSavedJobsQuery : IRequest<List<SavedJobDto>>
    {
        public Guid JobSeekerProfileId { get; set; }
    }
}
