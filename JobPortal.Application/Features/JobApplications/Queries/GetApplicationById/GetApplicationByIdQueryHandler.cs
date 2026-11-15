using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationById
{
    public class GetApplicationByIdQueryHandler
         : IRequestHandler<GetApplicationByIdQuery, JobApplicationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetApplicationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JobApplicationDto?> Handle(
            GetApplicationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var application = await _unitOfWork.JobApplications.GetApplicationByIdAsync(request.Id)
                ?? await _unitOfWork.JobApplications.GetByIdAsync(request.Id);

            if (application == null)
                return null;

            return new JobApplicationDto
            {
                Id = application.Id,
                JobId = application.JobId,
                JobTitle = application.Job?.Title,
                JobSeekerId = application.JobSeekerId,
                Name = application.Name,
                MobileNo = application.MobileNo,
                Email = application.Email,
                Gender = application.Gender,
                Education = application.Education,
                Language = application.Language,
                KeySkills = application.KeySkills,
                WorkExperience = application.WorkExperience,
                YearsOfExperience = application.YearsOfExperience,
                PreferredLocation = application.PreferredLocation,
                JobRole = application.JobRole,
                ResumeUrl = application.ResumeUrl,
                CoverLetter = application.CoverLetter,
                Status = application.Status,
                AppliedDate = application.AppliedOn
            };
        }
    }
}
