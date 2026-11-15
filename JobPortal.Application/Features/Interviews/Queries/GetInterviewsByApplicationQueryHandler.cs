using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Interviews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Queries
{
    public class GetInterviewsByApplicationQueryHandler
     : IRequestHandler<
         GetInterviewsByApplicationQuery,
         List<InterviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInterviewsByApplicationQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<InterviewDto>> Handle(
            GetInterviewsByApplicationQuery request,
            CancellationToken cancellationToken)
        {
            var interviews =
                await _unitOfWork.Interviews
                    .GetByApplicationIdAsync(
                        request.JobApplicationId);

            return _mapper.Map<
                List<InterviewDto>>(interviews);
        }
    }
}