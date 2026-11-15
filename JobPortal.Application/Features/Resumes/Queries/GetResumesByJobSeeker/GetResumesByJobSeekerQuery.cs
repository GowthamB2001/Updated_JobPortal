using JobPortal.Application.DTOs.Resumes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Resumes.Queries.GetResumesByJobSeeker
{
    public class GetResumesByJobSeekerQuery
         : IRequest<List<ResumeDto>>
    {
        public Guid JobSeekerId { get; set; }
    }
}
