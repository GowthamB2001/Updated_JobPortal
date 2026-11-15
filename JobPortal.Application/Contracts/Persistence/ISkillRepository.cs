using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface ISkillRepository
      : IGenericRepository<Skill>
    {
        Task<Skill?> GetByNameAsync(string skillName);
    }
}
