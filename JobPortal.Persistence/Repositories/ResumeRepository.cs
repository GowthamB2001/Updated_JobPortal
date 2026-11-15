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
    public class ResumeRepository
         : GenericRepository<Resume>,
           IResumeRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Resume>>
            GetByJobSeekerIdAsync(Guid jobSeekerId)
        {
            return await _context.Resumes
                .Where(x => x.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }
    }
}
