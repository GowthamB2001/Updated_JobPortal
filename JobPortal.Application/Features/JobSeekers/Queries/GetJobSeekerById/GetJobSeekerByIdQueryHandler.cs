using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Queries.GetJobSeekerById
{
    public class GetJobSeekerByIdQueryHandler
        : IRequestHandler<GetJobSeekerByIdQuery, JobSeekerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetJobSeekerByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<JobSeekerDto> Handle(
            GetJobSeekerByIdQuery request,
            CancellationToken cancellationToken)
        {
            var jobSeeker =
                await _unitOfWork.JobSeekers
                    .GetByIdAsync(request.Id);

            //if (jobSeeker == null)
            //    return null;

            return _mapper.Map<JobSeekerDto>(jobSeeker);
        }
    }
}
