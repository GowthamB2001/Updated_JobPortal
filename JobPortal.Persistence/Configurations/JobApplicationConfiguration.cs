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
    //public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    //{
    //    public void Configure(EntityTypeBuilder<JobApplication> builder)
    //    {
    //        builder.HasKey(x => x.Id);

    //        builder.HasOne(x => x.Job)
    //               .WithMany(x => x.Applications)
    //               .HasForeignKey(x => x.JobId)
    //               .OnDelete(DeleteBehavior.NoAction);

    //        builder.HasOne(x => x.JobSeeker)
    //               .WithMany(x => x.Applications)
    //               .HasForeignKey(x => x.JobSeekerId)
    //               .OnDelete(DeleteBehavior.NoAction);
    //    }
    //}
}
