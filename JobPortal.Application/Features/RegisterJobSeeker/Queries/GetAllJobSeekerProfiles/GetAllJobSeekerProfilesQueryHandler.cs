using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Queries.GetAllJobSeekerProfiles
{
    public class GetAllJobSeekerProfilesQueryHandler
         : IRequestHandler<GetAllJobSeekerProfilesQuery, List<JobSeekerProfileDto>>
    {
        private readonly IJobSeekerProfileRepository _repository;
        private readonly IMapper _mapper;

        public GetAllJobSeekerProfilesQueryHandler(
            IJobSeekerProfileRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<JobSeekerProfileDto>> Handle(
            GetAllJobSeekerProfilesQuery request,
            CancellationToken cancellationToken)
        {
            var profiles = await _repository.GetAllAsync();

            return _mapper.Map<List<JobSeekerProfileDto>>(profiles);
        }
    }
}
