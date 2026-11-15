using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Interviews
{
    public class InterviewDto
    {
        public Guid Id { get; set; }

        public Guid JobApplicationId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string MeetingLink { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }
    }
}
