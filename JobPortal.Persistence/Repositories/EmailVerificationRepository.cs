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
    public class EmailVerificationRepository
           : GenericRepository<EmailVerification>,
             IEmailVerificationRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailVerificationRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<EmailVerification?> GetByCompanyEmailAsync(
            string companyEmail)
        {
            return await _context.EmailVerifications
                .FirstOrDefaultAsync(x =>
                    x.CompanyEmail == companyEmail);
        }
    }
}