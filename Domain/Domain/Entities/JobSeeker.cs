using JobPortal.Domain.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Domain.Entities
{
    public class JobSeeker : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Location { get; set; }

        public User User { get; set; }

       
        public ICollection<JobSeekerSkill> JobSeekerSkills { get; set; }
    = new List<JobSeekerSkill>();

        //public ICollection<JobApplication> Applications { get; set; }
        //    = new List<JobApplication>();

        public ICollection<Resume> Resumes { get; set; }
            = new List<Resume>();
    }
}
