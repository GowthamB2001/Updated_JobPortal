using JobPortal.Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
   
        public class JobSkill : BaseEntity
        {
            public Guid JobId { get; set; }

            public Job Job { get; set; } = null!;

            public string SkillName { get; set; } = string.Empty;
        }
    }

