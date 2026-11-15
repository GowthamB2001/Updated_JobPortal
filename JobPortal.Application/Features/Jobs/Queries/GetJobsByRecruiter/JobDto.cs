using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter
{
    public class JobDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string SalaryRange { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public JobType JobType { get; set; }

        public string WorkMode { get; set; } = string.Empty;

        public int RepostCount { get; set; }

        public DateTime PostedOn { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<string> Skills { get; set; } = new();
    }
}
