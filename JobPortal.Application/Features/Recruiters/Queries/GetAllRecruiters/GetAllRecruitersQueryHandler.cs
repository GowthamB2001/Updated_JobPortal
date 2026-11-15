using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Recruiters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Queries.GetAllRecruiters
{
    public class GetAllRecruitersQueryHandler
       : IRequestHandler<GetAllRecruitersQuery,
           List<RecruiterDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRecruitersQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RecruiterDto>> Handle(
            GetAllRecruitersQuery request,
            CancellationToken cancellationToken)
        {
            var recruiters =
                await _unitOfWork.Recruiters.GetAllAsync();

            return _mapper.Map<List<RecruiterDto>>(recruiters);
        }
        }
}
