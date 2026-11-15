using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Queries.GetAllJobSeekers
{
    public class GetAllJobSeekersQueryHandler
        : IRequestHandler<GetAllJobSeekersQuery, List<JobSeekerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllJobSeekersQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<JobSeekerDto>> Handle(
            GetAllJobSeekersQuery request,
            CancellationToken cancellationToken)
        {
            var jobSeekers =
                await _unitOfWork.JobSeekers.GetAllAsync();

            return _mapper.Map<List<JobSeekerDto>>(jobSeekers);
        }
    }
}
