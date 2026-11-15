using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Recruiters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Queries.GetRecruiterById
{
    public class GetRecruiterByIdQueryHandler
        : IRequestHandler<GetRecruiterByIdQuery, RecruiterDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetRecruiterByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RecruiterDto> Handle(
        GetRecruiterByIdQuery request,
        CancellationToken cancellationToken)
        {
            var recruiter =
                await _unitOfWork.Recruiters
                    .GetByIdAsync(request.Id);

            return _mapper.Map<RecruiterDto>(recruiter);
        }
    }
}
