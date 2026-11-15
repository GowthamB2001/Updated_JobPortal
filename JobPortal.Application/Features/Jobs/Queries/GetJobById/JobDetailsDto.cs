using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetJobById
{
    public class JobDetailsDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string SalaryRange { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public JobType JobType { get; set; }

        public string WorkMode { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Openings { get; set; } = string.Empty;

        public string CompanyOverview { get; set; } = string.Empty;

        public DateTime PostedOn { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public List<string> Skills { get; set; } = new();
    }
}
