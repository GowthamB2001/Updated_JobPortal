using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Models.Email;
using JobPortal.Domain.Domain.Entities;
using JobPortal.Domain.Domain.Enums;
using MediatR;

namespace JobPortal.Application.Features.JobApplications.Commands.ApplyJob
{
    public class ApplyJobCommandHandler
        : IRequestHandler<ApplyJobCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public ApplyJobCommandHandler(
            IUnitOfWork unitOfWork,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Guid> Handle(
            ApplyJobCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Check Job Exists
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId);

            if (job == null)
                throw new Exception("Job not found.");

            // 2. Check Job Active
            if (!job.IsActive || job.ExpiryDate <= DateTime.UtcNow)
                throw new Exception("This job has expired.");

            // 3. Check Job Seeker Exists
            var jobSeeker = await _unitOfWork.JobSeekerProfile
                .GetByIdAsync(request.JobSeekerId);

            if (jobSeeker == null)
                throw new Exception("Job seeker not found.");

            // 4. Check Resume
            if (string.IsNullOrWhiteSpace(jobSeeker.ResumeUrl))
                throw new Exception("Please upload your resume before applying.");

            // 5. Duplicate Check
            var alreadyApplied = await _unitOfWork.JobApplications
                .AlreadyAppliedAsync(request.JobId, request.JobSeekerId);

            if (alreadyApplied)
                throw new Exception("You have already applied for this job.");

            // 6. Save Application Snapshot
            var application = new JobApplication
            {
                Id = Guid.NewGuid(),

                JobId = job.Id,
                JobSeekerId = jobSeeker.Id,

                Name = jobSeeker.Name,
                MobileNo = jobSeeker.MobileNo,
                Email = jobSeeker.Email ?? "",
                Gender = jobSeeker.Gender,
                Education = jobSeeker.Education,
                Language = jobSeeker.Language,
                KeySkills = jobSeeker.KeySkills,
                WorkExperience = jobSeeker.WorkExperience,
                YearsOfExperience = jobSeeker.YearsOfExperience,
                PreferredLocation = jobSeeker.PreferredLocation,
                JobRole = jobSeeker.JobRole,
                ResumeUrl = jobSeeker.ResumeUrl!,
                CoverLetter = request.CoverLetter,

                Status = ApplicationStatus.Pending,
                AppliedOn = DateTime.UtcNow
            };

            await _unitOfWork.JobApplications.AddAsync(application);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }

            // 7. Send Confirmation Email to Candidate
            if (!string.IsNullOrWhiteSpace(jobSeeker.Email))
            {
                var emailMessage = new EmailMessage
                {
                    To = jobSeeker.Email,
                    Subject = "Application Submitted Successfully",
                    Body = $@"
<div style='font-family:Arial,sans-serif'>
    <h2 style='color:#2563EB;'>Application Submitted Successfully</h2>

    <p>Hi <b>{jobSeeker.Name}</b>,</p>

    <p>Your application has been submitted successfully.</p>

    <table cellpadding='8' style='border-collapse:collapse'>
        <tr>
            <td><b>Job Title</b></td>
            <td>{job.Title}</td>
        </tr>

        <tr>
            <td><b>Company</b></td>
            <td>{job.Company}</td>
        </tr>

        <tr>
            <td><b>Status</b></td>
            <td>Pending</td>
        </tr>

        <tr>
            <td><b>Applied On</b></td>
            <td>{DateTime.Now:dd MMM yyyy hh:mm tt}</td>
        </tr>
    </table>

    <br/>

    <p>You can track your application status from the InterviewSpot app.</p>

    <br/>

    <p>Regards,<br/><b>InterviewSpot Team</b></p>
</div>"
                };

                await _emailService.SendEmailAsync(emailMessage);
            }

            return application.Id;
        }
    }
}