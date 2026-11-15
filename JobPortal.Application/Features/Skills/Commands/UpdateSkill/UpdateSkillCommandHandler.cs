using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Commands.UpdateSkill
{
    public class UpdateSkillCommandHandler
         : IRequestHandler<UpdateSkillCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSkillCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateSkillCommand request,
            CancellationToken cancellationToken)
        {
            var skill =
                await _unitOfWork.Skills
                    .GetByIdAsync(request.Id);

            if (skill == null)
                return false;

            skill.Name = request.Name;
            skill.Description = request.Description;

            await _unitOfWork.Skills
                .UpdateAsync(skill);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
