using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IRefreshTokenRepository _refreshTokens;

        public UnitOfWork(ApplicationDbContext context, IRefreshTokenRepository refreshTokenRepository)
        {
            _context = context;
            _refreshTokens = refreshTokenRepository;

            Users = new UserRepository(_context);
            Recruiters = new RecruiterRepository(_context);
            JobSeekerProfile = new JobSeekerProfileRepository(_context);
            JobSeekers = new JobSeekerRepository(_context);
            JobSeekerSkills = new JobSeekerSkillRepository(_context);
            Jobs = new JobRepository(_context);
            JobSkills = new JobSkillRepository(_context);
            JobApplications = new JobApplicationRepository(_context);
            Skills = new SkillRepository(_context);
            Resumes = new ResumeRepository(_context);
            JobRenewals = new JobRenewalRepository(_context);
            Interviews = new InterviewRepository(_context);
            GoogleLogin = new GoogleLoginRepository(_context);
            RefreshTokens =
        new RefreshTokenRepository(_context);
            Companies = new CompanyRepository(_context);

        }

        // Repositories
        public IUserRepository Users { get; }
        public IRecruiterRepository Recruiters { get; }
        public IJobSeekerRepository JobSeekers { get; }
        public IJobRepository Jobs { get; }
        public IJobSkillRepository JobSkills { get; }
        public IJobApplicationRepository JobApplications { get; }
        public ISkillRepository Skills { get; }
        public IResumeRepository Resumes { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public IJobRenewalRepository JobRenewals { get; }
        public IJobSeekerProfileRepository JobSeekerProfile { get; }
        public IJobSeekerSkillRepository JobSeekerSkills
        {
            get;
        }
        public IGoogleLoginRepository GoogleLogin { get; }
        public ICompanyRepository Companies { get; }
        public IInterviewRepository Interviews { get; }

        // Save all changes in one transaction
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose DbContext
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
