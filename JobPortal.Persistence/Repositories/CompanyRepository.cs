using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Persistence.Repositories
{
    public class CompanyRepository
        : GenericRepository<Company>,
          ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Company?> GetByOfficialEmailAsync(
            string email)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(x =>
                    x.OfficialEmail == email);
        }

        public async Task<Company?> GetByRecruiterIdAsync(
            Guid recruiterId)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(x =>
                    x.RecruiterId == recruiterId);
        }
    }
}