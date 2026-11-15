using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekerSkills.Commands.RemoveSkillFromJobSeeker
{
    public class RemoveSkillFromJobSeekerCommandHandler
        : IRequestHandler<RemoveSkillFromJobSeekerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveSkillFromJobSeekerCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            RemoveSkillFromJobSeekerCommand request,
            CancellationToken cancellationToken)
        {
            var mapping =
                await _unitOfWork.JobSeekerSkills
                    .GetAsync(
                        request.JobSeekerId,
                        request.SkillId);

            if (mapping == null)
                return false;

            await _unitOfWork.JobSeekerSkills
                .DeleteAsync(mapping);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
