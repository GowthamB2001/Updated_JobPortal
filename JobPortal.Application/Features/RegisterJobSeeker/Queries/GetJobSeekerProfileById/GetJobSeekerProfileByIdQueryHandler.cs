using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Queries.GetJobSeekerProfileById
{
    public class GetJobSeekerProfileByIdQueryHandler
        : IRequestHandler<GetJobSeekerProfileByIdQuery, JobSeekerProfileDto?>
    {
        private readonly IJobSeekerProfileRepository _repository;
        private readonly IMapper _mapper;

        public GetJobSeekerProfileByIdQueryHandler(
            IJobSeekerProfileRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<JobSeekerProfileDto?> Handle(
            GetJobSeekerProfileByIdQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.Id);

            if (profile == null)
                return null;

            return _mapper.Map<JobSeekerProfileDto>(profile);
        }
    }
}
