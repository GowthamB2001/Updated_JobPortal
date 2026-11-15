using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Commands.UpdateJobSeeker
{
    public class UpdateJobSeekerCommandHandler
       : IRequestHandler<UpdateJobSeekerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateJobSeekerCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateJobSeekerCommand request,
            CancellationToken cancellationToken)
        {
            var jobSeeker =
                await _unitOfWork.JobSeekers
                    .GetByIdAsync(request.Id);

            if (jobSeeker == null)
                return false;

            jobSeeker.UserId = request.UserId;
            jobSeeker.Location = request.Location;

            await _unitOfWork.JobSeekers
                .UpdateAsync(jobSeeker);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
