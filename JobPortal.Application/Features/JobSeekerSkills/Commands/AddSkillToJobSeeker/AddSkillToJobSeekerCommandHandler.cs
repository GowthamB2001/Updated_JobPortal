using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekerSkills.Commands.AddSkillToJobSeeker
{
    public class AddSkillToJobSeekerCommandHandler
        : IRequestHandler<AddSkillToJobSeekerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddSkillToJobSeekerCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            AddSkillToJobSeekerCommand request,
            CancellationToken cancellationToken)
        {
            var existing =
                await _unitOfWork.JobSeekerSkills
                    .GetAsync(
                        request.JobSeekerId,
                        request.SkillId);

            if (existing != null)
                return false;

            var mapping = new JobSeekerSkill
            {
                JobSeekerId = request.JobSeekerId,
                SkillId = request.SkillId
            };

            await _unitOfWork.JobSeekerSkills
                .AddAsync(mapping);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
