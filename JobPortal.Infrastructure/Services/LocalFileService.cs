using JobPortal.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Infrastructure.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> UploadResumeAsync(IFormFile? resume)
        {
            if (resume == null || resume.Length == 0)
                return null;

            var extension = Path.GetExtension(resume.FileName);

            if (!extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Only PDF files are allowed.");

            var webRoot = _environment.WebRootPath;

            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
            }

            var folderPath = Path.Combine(webRoot, "resumes");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await resume.CopyToAsync(stream);
            }

            return $"/resumes/{fileName}";
        }
    }
}