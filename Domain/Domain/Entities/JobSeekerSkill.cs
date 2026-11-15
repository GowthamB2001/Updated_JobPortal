using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class JobSeekerSkill
    {
        public Guid JobSeekerId { get; set; }

        public Guid SkillId { get; set; }

        public JobSeeker JobSeeker { get; set; }

        public Skill Skill { get; set; }
    }
}
