using JobPortal.Application.Contracts.Persistence;
using JobPortal.Persistence.Context;
using JobPortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.DependencyInjection
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IRecruiterRepository, RecruiterRepository>();

            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobSkillRepository, JobSkillRepository>();
            services.AddScoped<IJobSeekerRepository, JobSeekerRepository>();

            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();

            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IJobSeekerSkillRepository,
                   JobSeekerSkillRepository>();
            services.AddScoped<IGoogleLoginRepository, GoogleLoginRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();

            services.AddScoped<IJobRenewalRepository, JobRenewalRepository>();

            services.AddScoped<IEmailVerificationRepository,
    EmailVerificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IJobSeekerProfileRepository, JobSeekerProfileRepository>();
            return services;
        }
    }
}
