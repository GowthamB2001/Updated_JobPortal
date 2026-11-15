using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface IJobApplicationRepository
        : IGenericRepository<JobApplication>
    {
        Task<bool> AlreadyAppliedAsync(
            Guid jobId,
            Guid jobSeekerId);

        Task<IReadOnlyList<JobApplication>> GetApplicationsByJobAsync(
            Guid jobId);

        Task<IReadOnlyList<JobApplication>> GetApplicationsByJobSeekerAsync(
            Guid jobSeekerId);

        Task<JobApplication?> GetApplicationByIdAsync(
            Guid applicationId);
    }
}
