using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Repositories
{
    public class SavedJobRepository
        : GenericRepository<SavedJob>, ISavedJobRepository
    {
        private readonly ApplicationDbContext _context;

        public SavedJobRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<SavedJob>> GetByJobSeekerProfileIdAsync(Guid jobSeekerProfileId)
        {
            return await _context.SavedJobs
                .Include(s => s.Job!)
                    .ThenInclude(j => j.JobSkills)
                .Where(s => s.JobSeekerProfileId == jobSeekerProfileId && !s.IsDeleted && !s.Job!.IsDeleted)
                .OrderByDescending(s => s.CreatedOn)
                .ToListAsync();
        }

        public async Task<SavedJob?> GetByProfileAndJobIdAsync(Guid jobSeekerProfileId, Guid jobId)
        {
            return await _context.SavedJobs
                .FirstOrDefaultAsync(s => s.JobSeekerProfileId == jobSeekerProfileId && s.JobId == jobId && !s.IsDeleted);
        }

        public async Task<bool> IsJobSavedAsync(Guid jobSeekerProfileId, Guid jobId)
        {
            return await _context.SavedJobs
                .AnyAsync(s => s.JobSeekerProfileId == jobSeekerProfileId && s.JobId == jobId && !s.IsDeleted);
        }

        public async Task<IReadOnlyList<Guid>> GetSavedJobIdsAsync(Guid jobSeekerProfileId)
        {
            return await _context.SavedJobs
                .Where(s => s.JobSeekerProfileId == jobSeekerProfileId && !s.IsDeleted)
                .Select(s => s.JobId)
                .ToListAsync();
        }

        public async Task<SavedJob> SaveJobAsync(Guid jobSeekerProfileId, Guid jobId)
        {
            var existing = await _context.SavedJobs
                .FirstOrDefaultAsync(s => s.JobSeekerProfileId == jobSeekerProfileId && s.JobId == jobId);

            if (existing != null)
            {
                if (existing.IsDeleted)
                {
                    existing.IsDeleted = false;
                    existing.ModifiedOn = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                return existing;
            }

            var savedJob = new SavedJob
            {
                Id = Guid.NewGuid(),
                JobSeekerProfileId = jobSeekerProfileId,
                JobId = jobId,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await _context.SavedJobs.AddAsync(savedJob);
            await _context.SaveChangesAsync();

            return savedJob;
        }

        public async Task<bool> DeleteSavedJobAsync(Guid jobSeekerProfileId, Guid jobId)
        {
            var existing = await _context.SavedJobs
                .FirstOrDefaultAsync(s => s.JobSeekerProfileId == jobSeekerProfileId && s.JobId == jobId && !s.IsDeleted);

            if (existing == null)
                return false;

            existing.IsDeleted = true;
            existing.ModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
