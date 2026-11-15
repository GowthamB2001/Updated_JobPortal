using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Jobs
{
    public class JobListDto
    {
        public Guid Id { get; set; }

        // Client Required Fields
        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public List<string> KeySkills { get; set; } = new();

        public string SalaryRange { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public string? CompanyProfile { get; set; }

        public JobType JobType { get; set; }

        public string WorkMode { get; set; } = string.Empty;

        public DateTime PostedOn { get; set; }

        public string JobDescription { get; set; } = string.Empty;

        public string Openings { get; set; } = string.Empty;

        public string? CompanyOverview { get; set; }
    }
}
