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
    public class JobRenewalRepository : GenericRepository<JobRenewal>, IJobRenewalRepository
    {
        public JobRenewalRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<JobRenewal?> GetByJobIdAsync(Guid jobId)
        {
            return await _context.Set<JobRenewal>()
                .FirstOrDefaultAsync(x => x.JobId == jobId);
        }


    }
}
