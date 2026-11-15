using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.DeleteJob
{
    public class DeleteJobHandler
          : IRequestHandler<DeleteJobCommand, Unit>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteJobHandler(
            IJobRepository jobRepository,
            IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(
            DeleteJobCommand request,
            CancellationToken cancellationToken)
        {
            var job = await _jobRepository
                .GetJobByIdAndRecruiterAsync(
                    request.JobId,
                    request.RecruiterId);

            if (job == null)
                throw new Exception("Job not found.");

            await _jobRepository.DeleteAsync(job);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}