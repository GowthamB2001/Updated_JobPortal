using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Jobs
{
    public class JobSearchFilterDto
    {
        public string? Keyword { get; set; }

        public string? Location { get; set; }

        public JobType JobType { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
