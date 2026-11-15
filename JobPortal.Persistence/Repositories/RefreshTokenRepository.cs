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
    public class RefreshTokenRepository
         : GenericRepository<RefreshToken>,
           IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(
            string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(
                    x => x.Token == token);
        }
    }
}
