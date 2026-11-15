using JobPortal.Application.DTOs.Recruiters;
using MediatR;
using System;

namespace JobPortal.Application.Features.Recruiters.Queries.GetMyRecruiterProfile
{
    public class GetMyRecruiterProfileQuery : IRequest<RecruiterProfileDto?>
    {
        public Guid RecruiterId { get; set; }
    }
}
