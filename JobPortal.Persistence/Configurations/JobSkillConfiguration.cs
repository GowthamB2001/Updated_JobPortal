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
    //public class JobSkillConfiguration
    //: IEntityTypeConfiguration<JobSkill>
    //{
    //    public void Configure(EntityTypeBuilder<JobSkill> builder)
    //    {
    //        builder.HasKey(x => new
    //        {
    //            x.JobId,
    //            x.SkillId
    //        });

    //        builder.HasOne(x => x.Job)
    //               .WithMany(x => x.JobSkills)
    //               .HasForeignKey(x => x.JobId);

    //        builder.HasOne(x => x.Skill)
    //               .WithMany(x => x.JobSkills)
    //               .HasForeignKey(x => x.SkillId);
    //    }
    //}
}
