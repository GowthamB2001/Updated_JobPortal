using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Infrastructure
{
    public interface IFileService
    {
        Task<string?> UploadResumeAsync(IFormFile? resume);
    }
}
