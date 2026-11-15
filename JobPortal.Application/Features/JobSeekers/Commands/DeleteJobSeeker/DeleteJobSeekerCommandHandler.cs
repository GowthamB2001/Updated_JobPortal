using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Commands.DeleteJobSeeker
{
    public class DeleteJobSeekerCommandHandler
       : IRequestHandler<DeleteJobSeekerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteJobSeekerCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteJobSeekerCommand request,
            CancellationToken cancellationToken)
        {
            var jobSeeker =
                await _unitOfWork.JobSeekers
                    .GetByIdAsync(request.Id);

            if (jobSeeker == null)
                return false;

            await _unitOfWork.JobSeekers
                .DeleteAsync(jobSeeker);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
