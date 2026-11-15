using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Resumes.Commands.DeleteResume
{
    public class DeleteResumeCommandHandler
        : IRequestHandler<DeleteResumeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteResumeCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteResumeCommand request,
            CancellationToken cancellationToken)
        {
            var resume =
                await _unitOfWork.Resumes
                    .GetByIdAsync(request.Id);

            if (resume == null)
                return false;

            // Delete physical file
            if (!string.IsNullOrEmpty(resume.FilePath)
                && File.Exists(resume.FilePath))
            {
                File.Delete(resume.FilePath);
            }

            await _unitOfWork.Resumes
                .DeleteAsync(resume);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
