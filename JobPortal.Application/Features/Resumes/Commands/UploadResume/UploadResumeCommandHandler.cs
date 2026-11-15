using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Resumes.Commands.UploadResume
{
    public class UploadResumeCommandHandler
         : IRequestHandler<UploadResumeCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UploadResumeCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            UploadResumeCommand request,
            CancellationToken cancellationToken)
        {
            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                "Resumes");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(
                    uploadsFolder);
            }

            var fileName =
                $"{Guid.NewGuid()}_{request.File.FileName}";

            var filePath =
                Path.Combine(
                    uploadsFolder,
                    fileName);

            using (var stream =
                   new FileStream(
                       filePath,
                       FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var resume = new Resume
            {
                JobSeekerId = request.JobSeekerId,
                FileName = request.File.FileName,
                FilePath = filePath,
                FileType = request.File.ContentType,
                FileSize = request.File.Length,
                UploadedDate = DateTime.UtcNow
            };

            await _unitOfWork.Resumes
                .AddAsync(resume);

            await _unitOfWork.SaveChangesAsync();

            return resume.Id;
        }
    }
}
