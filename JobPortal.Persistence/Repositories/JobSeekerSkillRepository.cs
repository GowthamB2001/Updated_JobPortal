using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Repositories
{
    public class JobSeekerSkillRepository
         : GenericRepository<JobSeekerSkill>,
           IJobSeekerSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public JobSeekerSkillRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<JobSeekerSkill>>
            GetByJobSeekerIdAsync(Guid jobSeekerId)
        {
            return await _context.JobSeekerSkills
                .Where(x => x.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }

        public async Task<JobSeekerSkill?> GetAsync(
            Guid jobSeekerId,
            Guid skillId)
        {
            return await _context.JobSeekerSkills
                .FirstOrDefaultAsync(x =>
                    x.JobSeekerId == jobSeekerId &&
                    x.SkillId == skillId);
        }
    }
}
