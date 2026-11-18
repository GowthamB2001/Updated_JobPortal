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
    public class JobSeekerProfileRepository
        : GenericRepository<JobSeekerProfile>,
          IJobSeekerProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public JobSeekerProfileRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<JobSeekerProfile?> GetByMobileNoAsync(
            string mobileNo)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(x =>
                    x.MobileNo == mobileNo && !x.IsDeleted);
        }
        public async Task<JobSeekerProfile?> GetByEmailAsync(string email)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
        }
    }
}
