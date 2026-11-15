using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Resumes
{
    public class UploadResumeDto
    {
        public Guid JobSeekerId { get; set; }

        public IFormFile File { get; set; }
    }
}
