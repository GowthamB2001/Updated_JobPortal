using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Skills;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Queries.GetAllSkills
{
    public class GetAllSkillsQueryHandler
        : IRequestHandler<GetAllSkillsQuery, List<SkillDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSkillsQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SkillDto>> Handle(
            GetAllSkillsQuery request,
            CancellationToken cancellationToken)
        {
            var skills =
                await _unitOfWork.Skills.GetAllAsync();

            return _mapper.Map<List<SkillDto>>(skills);
        }
    }
}
