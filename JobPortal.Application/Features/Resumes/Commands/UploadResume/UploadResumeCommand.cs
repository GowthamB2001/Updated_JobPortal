using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Resumes.Commands.UploadResume
{
    public class UploadResumeCommand : IRequest<Guid>
    {
        public Guid JobSeekerId { get; set; }

        public IFormFile File { get; set; }
    }
}
