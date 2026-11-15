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
    public class JobApplicationRepository
      : GenericRepository<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> AlreadyAppliedAsync(
            Guid jobId,
            Guid jobSeekerId)
        {
            return await _context.JobApplications.AnyAsync(x =>
                x.JobId == jobId &&
                x.JobSeekerId == jobSeekerId);
        }

        public async Task<IReadOnlyList<JobApplication>> GetApplicationsByJobAsync(
            Guid jobId)
        {
            return await _context.JobApplications
                .Include(x => x.JobSeekerProfile)
                .Where(x => x.JobId == jobId)
                .OrderByDescending(x => x.AppliedOn)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<JobApplication>> GetApplicationsByJobSeekerAsync(
            Guid jobSeekerId)
        {
            return await _context.JobApplications
                .Include(x => x.Job)
                .Where(x => x.JobSeekerId == jobSeekerId)
                .OrderByDescending(x => x.AppliedOn)
                .ToListAsync();
        }

        public async Task<JobApplication?> GetApplicationByIdAsync(
            Guid applicationId)
        {
            return await _context.JobApplications
                .Include(x => x.Job)
                .Include(x => x.JobSeekerProfile)
                .FirstOrDefaultAsync(x => x.Id == applicationId);
        }
    }
}
