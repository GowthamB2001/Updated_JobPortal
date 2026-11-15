using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        IRecruiterRepository Recruiters { get; }

        IJobSeekerRepository JobSeekers { get; }

        IJobRepository Jobs { get; }
        IJobSkillRepository JobSkills { get; }
        IJobApplicationRepository JobApplications { get; }

        ISkillRepository Skills { get; }
        IJobSeekerProfileRepository JobSeekerProfile {  get; }
        IJobSeekerSkillRepository JobSeekerSkills { get; }
        IResumeRepository Resumes { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IJobRenewalRepository JobRenewals { get; }
        IInterviewRepository Interviews { get; }
        ICompanyRepository Companies { get; }
        IGoogleLoginRepository GoogleLogin { get; }
        Task<int> SaveChangesAsync();
    }
}
