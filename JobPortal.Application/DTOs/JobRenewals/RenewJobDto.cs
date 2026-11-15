using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.JobRenewals
{
    public class RenewJobDto
    {
        public Guid JobId { get; set; }

        public int RenewalDays { get; set; }
    }
}
