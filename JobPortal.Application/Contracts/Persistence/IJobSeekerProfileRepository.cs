using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface IJobSeekerProfileRepository
         : IGenericRepository<JobSeekerProfile>
    {
        Task<JobSeekerProfile?> GetByMobileNoAsync(string mobileNo);
        Task<JobSeekerProfile?> GetByEmailAsync(string email);
    }
}
