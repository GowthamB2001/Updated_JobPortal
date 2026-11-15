using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.RenewJob
{
    public class RenewJobHandler
       : IRequestHandler<RenewJobCommand, Unit>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RenewJobHandler(
            IJobRepository jobRepository,
            IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(
            RenewJobCommand request,
            CancellationToken cancellationToken)
        {
            var job = await _jobRepository
                .GetJobByIdAndRecruiterAsync(
                    request.JobId,
                    request.RecruiterId);

            if (job == null)
                throw new Exception("Job not found.");
            if (job.ExpiryDate > DateTime.UtcNow)
            {
                throw new Exception("Only expired jobs can be renewed.");
            }

            job.PostedOn = DateTime.UtcNow;

            job.ExpiryDate = DateTime.UtcNow.AddHours(24);

            job.RepostCount++;

            job.IsActive = true;

            await _jobRepository.UpdateAsync(job);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
