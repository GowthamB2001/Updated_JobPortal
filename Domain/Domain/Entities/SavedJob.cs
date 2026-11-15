using JobPortal.Domain.Domain.Common;
using System;

namespace JobPortal.Domain.Domain.Entities
{
    public class SavedJob : BaseEntity
    {
        public Guid JobSeekerProfileId { get; set; }
        public virtual JobSeekerProfile? JobSeekerProfile { get; set; }

        public Guid JobId { get; set; }
        public virtual Job? Job { get; set; }
    }
}
