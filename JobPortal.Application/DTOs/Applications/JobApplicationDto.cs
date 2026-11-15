using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Applications
{
    public class JobApplicationDto
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public Guid JobSeekerId { get; set; }

        public DateTime AppliedDate { get; set; }

        public ApplicationStatus Status { get; set; }

        public string? RecruiterRemarks { get; set; }
    }
}
