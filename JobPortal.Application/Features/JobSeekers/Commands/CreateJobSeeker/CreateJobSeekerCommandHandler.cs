using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Commands.CreateJobSeeker
{
    public class CreateJobSeekerCommandHandler
         : IRequestHandler<CreateJobSeekerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateJobSeekerCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateJobSeekerCommand request,
            CancellationToken cancellationToken)
        {
            var jobSeeker = new JobSeeker
            {
                UserId = request.UserId,
                Location = request.Location
            };

            await _unitOfWork.JobSeekers.AddAsync(jobSeeker);

            await _unitOfWork.SaveChangesAsync();

            return jobSeeker.Id;
        }
    }
}
