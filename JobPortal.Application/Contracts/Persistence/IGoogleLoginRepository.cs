using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface IGoogleLoginRepository
    {
        Task<GoogleLogin?> GetByGoogleIdAsync(string googleId);

        Task<GoogleLogin?> GetByEmailAsync(string email);

        Task AddAsync(GoogleLogin entity);

        Task UpdateAsync(GoogleLogin entity);
    }
}
