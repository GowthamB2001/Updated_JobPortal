using JobPortal.Application.DTOs.Skills;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekerSkills.Queries.GetSkillsByJobSeeker
{
    public class GetSkillsByJobSeekerQuery
        : IRequest<List<SkillDto>>
    {
        public Guid JobSeekerId { get; set; }
    }
}
