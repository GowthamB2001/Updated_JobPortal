using JobPortal.Domain.Domain.Entities;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface ICompanyRepository
        : IGenericRepository<Company>
    {
        Task<Company?> GetByOfficialEmailAsync(string email);

        Task<Company?> GetByRecruiterIdAsync(Guid recruiterId);
    }
}