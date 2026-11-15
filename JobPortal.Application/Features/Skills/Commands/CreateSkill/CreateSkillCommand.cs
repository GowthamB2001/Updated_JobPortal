using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Commands.CreateSkill
{
    public class CreateSkillCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
