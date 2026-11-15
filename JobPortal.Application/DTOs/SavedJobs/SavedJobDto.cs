using JobPortal.Application.DTOs.Jobs;
using System;

namespace JobPortal.Application.DTOs.SavedJobs
{
    public class SavedJobDto
    {
        public Guid SavedJobId { get; set; }
        public Guid JobSeekerProfileId { get; set; }
        public Guid JobId { get; set; }
        public DateTime SavedOn { get; set; }
        public JobListDto Job { get; set; } = null!;
    }

    public class SaveJobRequestDto
    {
        public Guid JobId { get; set; }
    }
}
