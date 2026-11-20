using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Queries.LoginJobSeeker
{
    public class JobSeekerLoginQueryHandler
        : IRequestHandler<JobSeekerLoginQuery, JobSeekerLoginResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public JobSeekerLoginQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<JobSeekerLoginResponse?> Handle(
            JobSeekerLoginQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await _unitOfWork.JobSeekerProfile
                .GetByMobileNoAsync(request.MobileNo);

            if (profile == null)
                return null;

            var response = _mapper.Map<JobSeekerLoginResponse>(profile);

            response.Token = _jwtTokenGenerator.GenerateToken(profile);
            response.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            return response;
        }
    }
}
