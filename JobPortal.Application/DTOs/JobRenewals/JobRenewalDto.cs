using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.JobRenewals
{
    public class JobRenewalDto
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public bool IsAutoRenewEnabled { get; set; }

        public int RenewalDays { get; set; }

        public DateTime RenewalEndDate { get; set; }

        public int RenewedCount { get; set; }
    }
}
