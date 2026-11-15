using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJob
{
    //public class GetApplicationsByJobQueryHandler
    //     : IRequestHandler<GetApplicationsByJobQuery,
    //         List<JobApplicationDto>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public GetApplicationsByJobQueryHandler(
    //        IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    public async Task<List<JobApplicationDto>> Handle(
    //        GetApplicationsByJobQuery request,
    //        CancellationToken cancellationToken)
    //    {
    //        var applications =
    //            await _unitOfWork.JobApplications
    //                .GetByJobIdAsync(request.JobId);
    //        return _mapper.Map<List<JobApplicationDto>>(applications);

    //        //    return applications.Select(x =>
    //        //        new JobApplicationDto
    //        //        {
    //        //            Id = x.Id,
    //        //            JobId = x.JobId,
    //        //            JobSeekerId = x.JobSeekerId,
    //        //            AppliedDate = x.AppliedDate,
    //        //            Status = x.Status,
    //        //            RecruiterRemarks = x.RecruiterRemarks
    //        //        }).ToList();
    //        //}
    //    }
    //}
}