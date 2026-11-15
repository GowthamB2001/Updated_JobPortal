using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Common;
using JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetPagedJobs
{
    public class GetPagedJobsQueryHandler
     : IRequestHandler<
         GetPagedJobsQuery,
         PagedResult<JobDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedJobsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<JobDto>> Handle(
            GetPagedJobsQuery request,
            CancellationToken cancellationToken)
        {
            var jobs =
                await _unitOfWork.Jobs
                    .GetPagedAsync(
                        request.PageNumber,
                        request.PageSize);

            var totalRecords =
      await _unitOfWork.Jobs.CountAsync();

            return new PagedResult<JobDto>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = totalRecords,
                Data = _mapper.Map<
                    List<JobDto>>(jobs)
            };
        }
    }
}
