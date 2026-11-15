using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Domain.Domain.Enums;
using MediatR;
using JobPortal.Domain.Domain.Enums;
using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Commands.CreateInterview
{
    public class CreateInterviewCommandHandler
        : IRequestHandler<
            CreateInterviewCommand,
            Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public CreateInterviewCommandHandler(
            IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Guid> Handle(
            CreateInterviewCommand request,
            CancellationToken cancellationToken)
        {
            var interview =
                new Interview
                {
                    JobApplicationId =
                        request.JobApplicationId,

                    InterviewDate =
                        request.InterviewDate,

                    MeetingLink =
                        request.MeetingLink,

                    Remarks =
                        request.Remarks,

                    Status =
                        InterviewStatus.Scheduled
                };

            await _unitOfWork.Interviews
                .AddAsync(interview);

            await _unitOfWork.SaveChangesAsync();

            // Send Email Notification

            var application =
                await _unitOfWork.JobApplications
                    .GetByIdAsync(
                        request.JobApplicationId);

            if (application != null)
            {
                var jobSeeker =
                    await _unitOfWork.JobSeekers
                        .GetByIdAsync(
                            application.JobSeekerId);

                if (jobSeeker != null)
                {
                    var user =
                        await _unitOfWork.Users
                            .GetByIdAsync(
                                jobSeeker.UserId);

                    if (user != null)
                    {
                        await _emailService.SendEmailAsync(
                            new EmailMessage
                            {
                                To = user.Email,
                                Subject = "Interview Scheduled",
                                Body =
                                $@"
                        Dear Candidate,<br/><br/>

                        Your interview has been scheduled.<br/><br/>

                        <b>Date:</b> {request.InterviewDate}<br/>
                        <b>Meeting Link:</b> {request.MeetingLink}<br/>
                        <b>Remarks:</b> {request.Remarks}<br/><br/>

                        Best Regards,<br/>
                        Job Portal Team
                        "
                            });
                    }
                }
            }

            return interview.Id;
        }
    }
}
