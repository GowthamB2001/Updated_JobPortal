using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Domain.Domain.Enums;
using JobPortal.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Repositories
{
    public class JobRepository
            : GenericRepository<Job>, IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }
        public async Task<List<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs
                .Include(x => x.JobSkills)
                    
                .Include(x => x.Recruiter)
                .OrderByDescending(x => x.PostedOn)
                .ToListAsync();
        }
        public async Task<IReadOnlyList<Job>> GetActiveJobsAsync()
        {
            return await _context.Jobs
                .Include(x => x.JobSkills)
                .Where(x =>
                    x.IsActive &&
                    x.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(x => x.PostedOn)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Job>> GetExpiredJobsAsync()
        {
            return await _context.Jobs
                .Include(x => x.JobSkills)
                .Where(x => !x.IsActive ||
                            x.ExpiryDate < DateTime.UtcNow)
                .OrderByDescending(x => x.PostedOn)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Job>> SearchJobsAsync(
     string? keyword,
     string? location,
     JobType? jobType,
     int pageNumber,
     int pageSize)
        {
            var query = _context.Jobs
                .Include(x => x.JobSkills)
                .Where(x =>
                    x.IsActive &&
                    x.ExpiryDate > DateTime.UtcNow)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.Title.Contains(keyword) ||
                    x.Company.Contains(keyword) ||
                    x.JobSkills.Any(s => s.SkillName.Contains(keyword)));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(x =>
                    x.Location.Contains(location));
            }

            if (jobType.HasValue)
            {
                query = query.Where(x =>
                    x.JobType == jobType.Value);
            }

            return await query
                .OrderByDescending(x => x.PostedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Job>> GetJobsByRecruiterAsync(Guid recruiterId)
        {
            return await _context.Jobs
                .Include(x => x.JobSkills)
                .Where(x => x.RecruiterId == recruiterId)
                .OrderByDescending(x => x.PostedOn)
                .ToListAsync();
        }

        public async Task<Job?> GetJobByIdAndRecruiterAsync(
            Guid jobId,
            Guid recruiterId)
        {
            return await _context.Jobs
                .Include(x => x.JobSkills)
                .FirstOrDefaultAsync(x =>
                    x.Id == jobId &&
                    x.RecruiterId == recruiterId);
        }



    }
}
