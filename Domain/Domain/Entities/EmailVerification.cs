using JobPortal.Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class EmailVerification : BaseEntity
    {
        public string CompanyEmail { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsVerified { get; set; } = false;
    }
}
