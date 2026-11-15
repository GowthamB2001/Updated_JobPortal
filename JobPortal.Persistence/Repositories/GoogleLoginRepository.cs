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
    public class GoogleLoginRepository : IGoogleLoginRepository
    {
        private readonly ApplicationDbContext _context;

        public GoogleLoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GoogleLogin?> GetByGoogleIdAsync(string googleId)
        {
            return await _context.GoogleLogins
                .FirstOrDefaultAsync(x => x.GoogleId == googleId);
        }

        public async Task<GoogleLogin?> GetByEmailAsync(string email)
        {
            return await _context.GoogleLogins
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task AddAsync(GoogleLogin entity)
        {
            await _context.GoogleLogins.AddAsync(entity);
        }

        public Task UpdateAsync(GoogleLogin entity)
        {
            _context.GoogleLogins.Update(entity);

            return Task.CompletedTask;
        }
    }
}
