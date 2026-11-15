using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Commands.DeleteSkill
{
    public class DeleteSkillCommandHandler
       : IRequestHandler<DeleteSkillCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSkillCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteSkillCommand request,
            CancellationToken cancellationToken)
        {
            var skill =
                await _unitOfWork.Skills
                    .GetByIdAsync(request.Id);

            if (skill == null)
                return false;

            await _unitOfWork.Skills
                .DeleteAsync(skill);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
