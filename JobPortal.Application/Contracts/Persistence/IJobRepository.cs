using JobPortal.Domain.Domain.Entities;
using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface IJobRepository
            : IGenericRepository<Job>
    {
        // Active Jobs
        Task<IReadOnlyList<Job>> GetActiveJobsAsync();

        // Expired Jobs
        Task<IReadOnlyList<Job>> GetExpiredJobsAsync();

        // Search Jobs
        Task<IReadOnlyList<Job>> SearchJobsAsync(
            string? keyword,
            string? location,
            JobType? jobType,
            int pageNumber,
            int pageSize);

        Task<List<Job>> GetAllJobsAsync();


        // Recruiter Dashboard
        Task<IReadOnlyList<Job>> GetJobsByRecruiterAsync(Guid recruiterId);



        // Get Single Job
        Task<Job?> GetJobByIdAndRecruiterAsync(
            Guid jobId,
            Guid recruiterId);
    }
}
