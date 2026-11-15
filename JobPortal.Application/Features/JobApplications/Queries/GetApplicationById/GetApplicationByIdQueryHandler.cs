using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationById
{
    public class GetApplicationByIdQueryHandler
         : IRequestHandler<GetApplicationByIdQuery, JobApplicationDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetApplicationByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JobApplicationDto> Handle(
            GetApplicationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var application =
                await _unitOfWork.JobApplications
                    .GetByIdAsync(request.Id);

            if (application == null)
                return null;

            return new JobApplicationDto
            {
                Id = application.Id,
                JobId = application.JobId,
                JobSeekerId = application.JobSeekerId,
                AppliedDate = application.AppliedOn,
                Status = application.Status,
                //RecruiterRemarks = application.RecruiterRemarks
            };
        }
    }
}