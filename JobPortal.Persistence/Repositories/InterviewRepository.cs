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
    public class InterviewRepository
        : GenericRepository<Interview>,
          IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Interview>>
            GetByApplicationIdAsync(
                Guid applicationId)
        {
            return await _context.Interviews
                .Where(x =>
                    x.JobApplicationId ==
                    applicationId)
                .ToListAsync();
        }
    }
}
