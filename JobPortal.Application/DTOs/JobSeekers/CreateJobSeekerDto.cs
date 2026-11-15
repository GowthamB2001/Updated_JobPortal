using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.JobSeekers
{
    public class CreateJobSeekerDto
    {
        public Guid UserId { get; set; }

        public string Location { get; set; }
    }
}
