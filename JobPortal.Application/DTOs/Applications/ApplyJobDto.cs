using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Applications
{
    public class ApplyJobDto
    {
        public Guid JobId { get; set; }

        public Guid JobSeekerId { get; set; }
    }
}
