using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.JobSeekers
{
    public class JobSeekerDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Location { get; set; }
    }
}
