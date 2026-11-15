using JobPortal.Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    
        public class Skill : BaseEntity
        {
            public string Name { get; set; } = string.Empty;

            public string? Description { get; set; }

            public ICollection<JobSeekerSkill> JobSeekerSkills { get; set; }
                = new List<JobSeekerSkill>();

            public ICollection<JobSkill> JobSkills { get; set; }
                = new List<JobSkill>();
        }
    }

