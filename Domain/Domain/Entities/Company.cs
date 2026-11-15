using JobPortal.Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }

        public string Website { get; set; }

        public string Address { get; set; }

        public string OfficialEmail { get; set; }

        public bool IsEmailVerified { get; set; }

        public string? EmailOtp { get; set; }

        public DateTime? OtpExpiry { get; set; }

        public Guid RecruiterId { get; set; }

        public User Recruiter { get; set; }
    }
}
