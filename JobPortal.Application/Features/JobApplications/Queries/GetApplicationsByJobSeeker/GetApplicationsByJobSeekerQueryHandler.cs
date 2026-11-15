using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJobSeeker
{
    //public class GetApplicationsByJobSeekerQueryHandler
    //    : IRequestHandler<GetApplicationsByJobSeekerQuery,
    //        List<JobApplicationDto>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;

    //    public GetApplicationsByJobSeekerQueryHandler(
    //        IUnitOfWork unitOfWork)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public async Task<List<JobApplicationDto>> Handle(
    //        GetApplicationsByJobSeekerQuery request,
    //        CancellationToken cancellationToken)
    //    {
    //        var applications =
    //            await _unitOfWork.JobApplications
    //                .GetByJobSeekerIdAsync(
    //                    request.JobSeekerId);

    //        return applications.Select(x =>
    //            new JobApplicationDto
    //            {
    //                Id = x.Id,
    //                JobId = x.JobId,
    //                JobSeekerId = x.JobSeekerId,
    //                AppliedDate = x.AppliedDate,
    //                Status = x.Status,
    //                RecruiterRemarks = x.RecruiterRemarks
    //            }).ToList();
    //    }
    //}
}