using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface IJobSeekerSkillRepository
        : IGenericRepository<JobSeekerSkill>
    {
        Task<List<JobSeekerSkill>> GetByJobSeekerIdAsync(
            Guid jobSeekerId);

        Task<JobSeekerSkill?> GetAsync(
            Guid jobSeekerId,
            Guid skillId);
    }
}
