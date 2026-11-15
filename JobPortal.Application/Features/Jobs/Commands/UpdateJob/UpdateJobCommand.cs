using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.Jobs.Commands.UpdateJob
{
    public class UpdateJobCommand : IRequest<Unit>
    {
        public Guid JobId { get; set; }

        public Guid RecruiterId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public List<string> KeySkills { get; set; } = new();

        public string? SalaryRange { get; set; }

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }

        public string Currency { get; set; } = "INR";

        public string Experience { get; set; } = string.Empty;

        public JobType JobType { get; set; }

        public string WorkMode { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Openings { get; set; } = string.Empty;

        public string CompanyOverview { get; set; } = string.Empty;

        public int AutoRepostDays { get; set; } = 1;

        public bool IsDraft { get; set; } = false;
    }
}
