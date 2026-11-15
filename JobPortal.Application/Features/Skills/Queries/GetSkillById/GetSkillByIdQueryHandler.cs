using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Skills;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Skills.Queries.GetSkillById
{
    public class GetSkillByIdQueryHandler
        : IRequestHandler<GetSkillByIdQuery, SkillDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetSkillByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SkillDto> Handle(
            GetSkillByIdQuery request,
            CancellationToken cancellationToken)
        {
            var skill =
                await _unitOfWork.Skills.GetByIdAsync(request.Id);

          

            return _mapper.Map<SkillDto>(skill);
        }
    }
}
