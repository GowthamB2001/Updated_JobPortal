using JobPortal.Domain.Domain.Entities;
using JobPortal.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<GoogleLogin> GoogleLogins { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobSeekerSkill> JobSeekerSkills { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<JobRenewal> JobRenewals { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<Interview>()
    .HasOne(i => i.JobApplication)
    .WithMany()
    .HasForeignKey(i => i.JobApplicationId);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<JobSkill>()
    .HasOne(js => js.Job)
    .WithMany(j => j.JobSkills)
    .HasForeignKey(js => js.JobId)
    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<JobApplication>()
    .HasOne(x => x.Job)
    .WithMany(x => x.JobApplications)
    .HasForeignKey(x => x.JobId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobApplication>()
                .HasOne(x => x.JobSeekerProfile)
                .WithMany(x => x.JobApplications)
                .HasForeignKey(x => x.JobSeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedJob>()
                .HasOne(x => x.Job)
                .WithMany()
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedJob>()
                .HasOne(x => x.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(x => x.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.ApplyConfiguration(new GoogleLoginConfiguration());
        }


    } }