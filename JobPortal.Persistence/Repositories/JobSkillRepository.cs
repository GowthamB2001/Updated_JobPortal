using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Repositories
{
    public class JobSkillRepository
         : GenericRepository<JobSkill>, IJobSkillRepository
    {
        public JobSkillRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
