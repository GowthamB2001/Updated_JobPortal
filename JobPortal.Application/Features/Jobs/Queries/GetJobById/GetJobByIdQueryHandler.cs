using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetJobById
{
    public class GetJobByIdQueryHandler
         : IRequestHandler<GetJobByIdQuery, JobDetailsDto?>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public GetJobByIdQueryHandler(
            IJobRepository jobRepository,
            IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<JobDetailsDto?> Handle(
            GetJobByIdQuery request,
            CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.Id);

            if (job == null)
                return null;

            return _mapper.Map<JobDetailsDto>(job);
        }
    }
}
