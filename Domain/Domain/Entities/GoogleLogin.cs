using JobPortal.Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class GoogleLogin : BaseEntity
    {
        public string GoogleId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Guid? JobSeekerProfileId { get; set; }

        public JobSeekerProfile? JobSeekerProfile { get; set; }
    }
}
