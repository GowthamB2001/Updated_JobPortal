using JobPortal.Application.Contracts.Persistence;
using MediatR;

namespace JobPortal.Application.Features.RegisterJobSeeker.Command.DeleteJobSeekerProfile
{
    public class DeleteJobSeekerProfileCommandHandler
        : IRequestHandler<DeleteJobSeekerProfileCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteJobSeekerProfileCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            DeleteJobSeekerProfileCommand request,
            CancellationToken cancellationToken)
        {
            var profile = await _unitOfWork.JobSeekerProfile
                .GetByIdAsync(request.Id);

            if (profile == null)
                throw new Exception("Job Seeker not found.");

            if (profile.IsDeleted)
                throw new Exception("Account already deleted.");

            // Soft delete
            profile.IsDeleted = true;
            profile.ModifiedOn = DateTime.UtcNow;

            // DO NOT use DeleteAsync()
            await _unitOfWork.JobSeekerProfile
                .UpdateAsync(profile);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}