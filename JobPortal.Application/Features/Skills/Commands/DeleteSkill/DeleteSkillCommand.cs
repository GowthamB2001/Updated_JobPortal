using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Commands.DeleteSkill
{
    public class DeleteSkillCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
