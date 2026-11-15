using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandHandler
        : IRequestHandler<UpdateApplicationStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateApplicationStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateApplicationStatusCommand request,
            CancellationToken cancellationToken)
        {
            var application = await _unitOfWork.JobApplications.GetByIdAsync(request.ApplicationId);

            if (application == null)
                return false;

            application.Status = request.Status;
            application.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.JobApplications.UpdateAsync(application);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
