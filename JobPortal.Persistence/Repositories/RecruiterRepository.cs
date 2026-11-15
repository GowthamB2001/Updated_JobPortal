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
    public class RecruiterRepository
        : GenericRepository<Recruiter>, IRecruiterRepository
    {
        private readonly ApplicationDbContext _context;

        public RecruiterRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Recruiter?> GetByOfficialEmailAsync(string officialEmail)
        {
            return await _context.Recruiters
                .FirstOrDefaultAsync(x => x.OfficialEmail == officialEmail);
        }

        public async Task<bool> ExistsByOfficialEmailAsync(string officialEmail)
        {
            return await _context.Recruiters
                .AnyAsync(x => x.OfficialEmail == officialEmail);
        }
    }
}
