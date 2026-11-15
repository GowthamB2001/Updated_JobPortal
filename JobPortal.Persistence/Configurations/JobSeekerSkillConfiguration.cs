using JobPortal.Domain.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Persistence.Configurations
{
    public class JobSeekerSkillConfiguration
        : IEntityTypeConfiguration<JobSeekerSkill>
    {
        public void Configure(
            EntityTypeBuilder<JobSeekerSkill> builder)
        {
            builder.HasKey(x =>
                new
                {
                    x.JobSeekerId,
                    x.SkillId
                });

            builder.HasOne(x => x.JobSeeker)
                .WithMany(x => x.JobSeekerSkills)
                .HasForeignKey(x => x.JobSeekerId);

            builder.HasOne(x => x.Skill)
                .WithMany(x => x.JobSeekerSkills)
                .HasForeignKey(x => x.SkillId);
        }
    }
}
