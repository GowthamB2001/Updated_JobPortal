using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.SavedJobs.Commands.DeleteSavedJob
{
    public class DeleteSavedJobCommandHandler : IRequestHandler<DeleteSavedJobCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSavedJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteSavedJobCommand request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.SavedJobs.DeleteSavedJobAsync(request.JobSeekerProfileId, request.JobId);
        }
    }
}
