using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekerSkills.Commands.AddSkillToJobSeeker
{
    public class AddSkillToJobSeekerCommand : IRequest<bool>
    {
        public Guid JobSeekerId { get; set; }

        public Guid SkillId { get; set; }
    }
}
