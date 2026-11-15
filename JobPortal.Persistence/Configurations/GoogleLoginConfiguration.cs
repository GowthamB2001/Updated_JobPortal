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
    public class GoogleLoginConfiguration
        : IEntityTypeConfiguration<GoogleLogin>
    {
        public void Configure(EntityTypeBuilder<GoogleLogin> builder)
        {
            builder.ToTable("GoogleLogins");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.GoogleId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(x => x.GoogleId)
                .IsUnique();

            builder.HasIndex(x => x.Email);

            builder.HasOne(x => x.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(x => x.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}