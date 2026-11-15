using JobPortal.Application.DTOs.Skills;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Queries.GetAllSkills
{
    public class GetAllSkillsQuery
        : IRequest<List<SkillDto>>
    {
    }
}
