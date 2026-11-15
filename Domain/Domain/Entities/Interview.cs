using JobPortal.Domain.Domain.Common;
using JobPortal.Domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class Interview : BaseEntity
    {
        public Guid JobApplicationId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string MeetingLink { get; set; }

        public string Remarks { get; set; }

       
        public InterviewStatus Status { get; set; }

        public JobApplication JobApplication { get; set; }
    }
}
