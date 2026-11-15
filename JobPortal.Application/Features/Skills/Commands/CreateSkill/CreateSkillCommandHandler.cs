using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Commands.CreateSkill
{
    public class CreateSkillCommandHandler
         : IRequestHandler<CreateSkillCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSkillCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateSkillCommand request,
            CancellationToken cancellationToken)
        {
            var skill = new Skill
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Skills.AddAsync(skill);

            await _unitOfWork.SaveChangesAsync();

            return skill.Id;
        }
    }
}
