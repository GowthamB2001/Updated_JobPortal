using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Skills;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekerSkills.Queries.GetSkillsByJobSeeker
{
    public class GetSkillsByJobSeekerQueryHandler
        : IRequestHandler<GetSkillsByJobSeekerQuery,
            List<SkillDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSkillsByJobSeekerQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SkillDto>> Handle(
            GetSkillsByJobSeekerQuery request,
            CancellationToken cancellationToken)
        {
            var mappings =
                await _unitOfWork.JobSeekerSkills
                    .GetByJobSeekerIdAsync(
                        request.JobSeekerId);

            var result = new List<SkillDto>();

            foreach (var item in mappings)
            {
                var skill =
                    await _unitOfWork.Skills
                        .GetByIdAsync(item.SkillId);

                if (skill != null)
                {
                    result.Add(new SkillDto
                    {
                        Id = skill.Id,
                        Name = skill.Name,
                        Description = skill.Description
                    });
                }
            }

            return result;
        }
    }
}
