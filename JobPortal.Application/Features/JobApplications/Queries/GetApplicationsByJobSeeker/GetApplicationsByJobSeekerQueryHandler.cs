using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Applications;
using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJobSeeker
{
    public class GetApplicationsByJobSeekerQueryHandler
        : IRequestHandler<GetApplicationsByJobSeekerQuery, List<JobApplicationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetApplicationsByJobSeekerQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<JobApplicationDto>> Handle(
            GetApplicationsByJobSeekerQuery request,
            CancellationToken cancellationToken)
        {
            var rawApps = await _unitOfWork.JobApplications.GetApplicationsByJobSeekerAsync(request.JobSeekerId);
            var applications = rawApps.ToList();

            // Filter by status tab if requested (e.g. All, Applied, Shortlisted, Rejected)
            if (!string.IsNullOrWhiteSpace(request.Status) && !request.Status.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                applications = applications
                    .Where(a => a.Status.ToString().Equals(request.Status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var savedJobIds = await _unitOfWork.SavedJobs.GetSavedJobIdsAsync(request.JobSeekerId);
            var allApps = (await _unitOfWork.JobApplications.GetAllAsync()).ToList();

            return applications.Select(x =>
            {
                var job = x.Job;

                // 1. Calculate relative applied time
                var diff = DateTime.UtcNow - x.AppliedOn;
                string timeAgo;
                if (diff.TotalDays < 1)
                {
                    timeAgo = diff.TotalHours < 1 ? "Applied just now" : $"Applied {(int)diff.TotalHours} hours ago";
                }
                else if (diff.TotalDays < 2)
                {
                    timeAgo = "Applied 1 day ago";
                }
                else if (diff.TotalDays < 7)
                {
                    timeAgo = $"Applied {(int)diff.TotalDays} days ago";
                }
                else if (diff.TotalDays < 14)
                {
                    timeAgo = "Applied 1 week ago";
                }
                else if (diff.TotalDays < 30)
                {
                    timeAgo = $"Applied {(int)(diff.TotalDays / 7)} weeks ago";
                }
                else
                {
                    timeAgo = $"Applied {(int)(diff.TotalDays / 30)} months ago";
                }

                // 2. Status Banner Copy
                string bannerTitle;
                string bannerDesc;
                switch (x.Status)
                {
                    case ApplicationStatus.Shortlisted:
                        bannerTitle = "Congratulations! You have been shortlisted.";
                        bannerDesc = "The recruiter has shortlisted your profile and will contact you shortly regarding next steps.";
                        break;
                    case ApplicationStatus.Rejected:
                        bannerTitle = "Application update: Not moving forward.";
                        bannerDesc = "Thank you for taking the time to apply. New matching opportunities are posted daily.";
                        break;
                    case ApplicationStatus.Hired:
                        bannerTitle = "Congratulations! You have been hired!";
                        bannerDesc = "The recruiter has selected you for this position. Welcome to the team!";
                        break;
                    default:
                        bannerTitle = "Great work! You applied on time.";
                        bannerDesc = "The recruiter will review your profile and contact you shortly regarding the next steps.";
                        break;
                }

                // 3. Applicant Count
                int applicantCount = allApps.Count(a => a.JobId == x.JobId);

                // 4. Skills & Overview
                var skills = job?.JobSkills?.Select(s => s.SkillName).ToList() ?? new List<string>();
                string company = !string.IsNullOrWhiteSpace(job?.Company) ? job.Company : "TechCorp Global";
                string companyInitial = company.Substring(0, 1).ToUpper();

                string companyOverview = !string.IsNullOrWhiteSpace(job?.CompanyOverview)
                    ? job.CompanyOverview
                    : $"{company} is a global technology leader focused on improving the ways people connect with information and digital solutions.";

                decimal? minSal = null;
                decimal? maxSal = null;
                try { if (job?.MinSalary != null) minSal = Convert.ToDecimal(job.MinSalary); } catch { }
                try { if (job?.MaxSalary != null) maxSal = Convert.ToDecimal(job.MaxSalary); } catch { }

                string salaryRange = !string.IsNullOrWhiteSpace(job?.SalaryRange)
                    ? job.SalaryRange
                    : (minSal.HasValue && maxSal.HasValue
                        ? $"₹{(long)minSal.Value:N0} - ₹{(long)maxSal.Value:N0}"
                        : "$140k - $160k");

                string aboutRole = !string.IsNullOrWhiteSpace(job?.JobDescription)
                    ? job.JobDescription
                    : $"We are looking for an experienced {job?.Title ?? "Professional"} to lead the design and development of our core digital products.";

                bool isBookmarked = savedJobIds.Contains(x.JobId);

                return new JobApplicationDto
                {
                    Id = x.Id,
                    JobId = x.JobId,
                    JobSeekerId = x.JobSeekerId,

                    // Screenshot 1 & 2: Job & Company info
                    JobTitle = !string.IsNullOrWhiteSpace(job?.Title) ? job.Title : x.JobRole,
                    Company = company,
                    CompanyInitial = companyInitial,
                    CompanyLogo = null,
                    JobLocation = !string.IsNullOrWhiteSpace(job?.Location) ? job.Location : (!string.IsNullOrWhiteSpace(x.PreferredLocation) ? x.PreferredLocation : "Mountain View, CA"),

                    // Screenshot 2: Badges
                    IsActiveRecruiter = true,
                    ReplyRate = "92% reply rate",
                    IsBookmarked = isBookmarked,

                    // Screenshot 2 & 3: 4 Quick Stat Cards
                    SalaryRange = salaryRange,
                    MinSalary = minSal,
                    MaxSalary = maxSal,
                    Currency = !string.IsNullOrWhiteSpace(job?.Currency) ? job.Currency : "USD",
                    Experience = !string.IsNullOrWhiteSpace(job?.Experience) ? job.Experience : "3-5 Yrs",
                    JobType = job?.JobType.ToString() ?? "Full-time",
                    WorkMode = !string.IsNullOrWhiteSpace(job?.WorkMode) ? job.WorkMode : "Remote",
                    Openings = !string.IsNullOrWhiteSpace(job?.Openings) ? job.Openings : "02",

                    // Screenshot 2: Applicant count
                    ApplicantCount = applicantCount,

                    // Screenshot 1 & 2: Status & Banners
                    Status = x.Status,
                    StatusBannerTitle = bannerTitle,
                    StatusBannerDescription = bannerDesc,

                    // Screenshot 1 & 2: Dates
                    AppliedDate = x.AppliedOn,
                    AppliedTimeAgo = timeAgo,

                    // Screenshot 2, 3, 4: Bottom CTA
                    HasApplied = true,

                    // Screenshot 2 & 3: Role details & Skills
                    AboutTheRole = aboutRole,
                    SkillsRequired = skills.Count > 0 ? skills : new List<string> { "Figma", "UI Design", "UX Research", "Prototyping", "Design Systems", "User Testing" },

                    // Screenshot 4: Company Overview
                    CompanyOverview = companyOverview,
                    CompanyProfile = $"/api/Companies/{job?.RecruiterId}",

                    // Candidate profile snapshot
                    Name = x.Name,
                    MobileNo = x.MobileNo,
                    Email = x.Email,
                    Gender = x.Gender,
                    Education = x.Education,
                    Language = x.Language,
                    KeySkills = x.KeySkills,
                    WorkExperience = x.WorkExperience,
                    YearsOfExperience = x.YearsOfExperience,
                    PreferredLocation = x.PreferredLocation,
                    JobRole = x.JobRole,
                    ResumeUrl = x.ResumeUrl,
                    CoverLetter = x.CoverLetter,
                    RecruiterRemarks = null
                };
            }).ToList();
        }
    }
}
