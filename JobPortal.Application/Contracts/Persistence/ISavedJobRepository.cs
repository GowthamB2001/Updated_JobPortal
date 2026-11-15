using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface ISavedJobRepository : IGenericRepository<SavedJob>
    {
        Task<IReadOnlyList<SavedJob>> GetByJobSeekerProfileIdAsync(Guid jobSeekerProfileId);
        Task<SavedJob?> GetByProfileAndJobIdAsync(Guid jobSeekerProfileId, Guid jobId);
        Task<bool> IsJobSavedAsync(Guid jobSeekerProfileId, Guid jobId);
        Task<IReadOnlyList<Guid>> GetSavedJobIdsAsync(Guid jobSeekerProfileId);
        Task<SavedJob> SaveJobAsync(Guid jobSeekerProfileId, Guid jobId);
        Task<bool> DeleteSavedJobAsync(Guid jobSeekerProfileId, Guid jobId);
    }
}
