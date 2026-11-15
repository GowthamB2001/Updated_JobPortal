using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;

namespace JobPortal.Application.Features.RegisterJobSeeker.Queries.GetJobSeekerProfileById
{
    public class GetJobSeekerProfileByIdQuery : IRequest<JobSeekerProfileDto?>
    {
        public Guid Id { get; set; }
    }
}
