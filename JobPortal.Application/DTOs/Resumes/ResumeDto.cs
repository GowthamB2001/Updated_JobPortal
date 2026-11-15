using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Resumes
{
    public class ResumeDto
    {
        public Guid Id { get; set; }

        public Guid JobSeekerId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileType { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedDate { get; set; }
    }
}
