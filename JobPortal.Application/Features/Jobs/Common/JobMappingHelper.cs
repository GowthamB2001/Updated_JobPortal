using JobPortal.Application.DTOs.Jobs;
using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobPortal.Application.Features.Jobs.Common
{
    public static class JobMappingHelper
    {
        public static JobListDto MapToJobListDto(
            Job job,
            JobSeekerProfile? profile,
            IReadOnlyCollection<JobApplication> allApplications,
            IReadOnlyCollection<Guid>? savedJobIds = null)
        {
            var skills = job.JobSkills?.Select(x => x.SkillName).ToList() ?? new List<string>();

            // Calculate Match Percentage
            int matchPercentage = 70;
            if (profile != null)
            {
                var seekerSkills = (profile.KeySkills ?? "")
                    .Split(new[] { ',', ';', '/', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().ToLower())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                var jobSkillsLower = skills
                    .Select(s => s.Trim().ToLower())
                    .ToList();

                int matchedCount = 0;
                foreach (var js in jobSkillsLower)
                {
                    if (seekerSkills.Any(ss => js.Contains(ss) || ss.Contains(js)))
                    {
                        matchedCount++;
                    }
                }

                double skillRatio = jobSkillsLower.Count > 0
                    ? (double)matchedCount / jobSkillsLower.Count
                    : 0.5;

                bool roleMatches = false;
                if (!string.IsNullOrWhiteSpace(profile.JobRole) && !string.IsNullOrWhiteSpace(job.Title))
                {
                    var pRole = profile.JobRole.Trim().ToLower();
                    var jTitle = job.Title.Trim().ToLower();
                    roleMatches = jTitle.Contains(pRole) || pRole.Contains(jTitle);
                }

                double score = (skillRatio * 55.0) + (roleMatches ? 35.0 : 15.0);
                matchPercentage = (int)Math.Clamp(Math.Round(score), 50, 95);
            }

            // Expiry & Countdown
            var remainingSpan = job.ExpiryDate > DateTime.UtcNow
                ? job.ExpiryDate - DateTime.UtcNow
                : TimeSpan.Zero;

            double remainingSeconds = Math.Max(0, remainingSpan.TotalSeconds);
            int totalHoursInt = (int)Math.Max(0, Math.Floor(remainingSpan.TotalHours));
            int days = (int)Math.Max(0, remainingSpan.TotalDays);
            int hours = Math.Max(0, remainingSpan.Hours);
            int minutes = Math.Max(0, remainingSpan.Minutes);
            int seconds = Math.Max(0, remainingSpan.Seconds);

            string closesIn = remainingSpan.TotalHours >= 24
                ? $"{days}d {hours:D2}:{minutes:D2}:{seconds:D2}"
                : $"{totalHoursInt:D2}:{minutes:D2}:{seconds:D2}";

            // Applicants
            int applicantCount = allApplications?.Count(a => a.JobId == job.Id) ?? 0;
            if (applicantCount == 0)
            {
                // Fallback realistic display count if no applications yet
                applicantCount = (Math.Abs(job.Id.GetHashCode()) % 20) + 5;
            }

            bool hasApplied = profile != null && (allApplications?.Any(a => a.JobId == job.Id && a.JobSeekerId == profile.Id) ?? false);

            var companyOverview = !string.IsNullOrWhiteSpace(job.CompanyOverview)
                ? job.CompanyOverview
                : $"{job.Company} is a leading provider of enterprise software solutions, focusing on innovative technologies that drive business growth.";

            return new JobListDto
            {
                Id = job.Id,
                Title = job.Title,
                Company = job.Company,
                Location = job.Location,
                CompanyLogo = null,
                CompanyOverview = companyOverview,
                CompanyProfile = companyOverview,

                MatchPercentage = matchPercentage,
                ProfileMatchDescription = matchPercentage >= 75
                    ? "Your skills align well with this opportunity."
                    : "Good match based on your primary experience and location.",
                IsActiveRecruiter = true,
                FastReply = true,
                ReplyRate = "92% reply rate",
                IsActivelyHiring = true,
                IsBookmarked = savedJobIds != null && savedJobIds.Contains(job.Id),

                SalaryRange = !string.IsNullOrWhiteSpace(job.SalaryRange)
                    ? job.SalaryRange
                    : (job.MinSalary.HasValue && job.MaxSalary.HasValue ? $"₹{Convert.ToInt64(job.MinSalary.Value):N0} - ₹{Convert.ToInt64(job.MaxSalary.Value):N0}" : "₹5,00,000 - ₹15,00,000"),
                MinSalary = job.MinSalary ?? 500000,
                MaxSalary = job.MaxSalary ?? 1500000,
                Currency = string.IsNullOrWhiteSpace(job.Currency) ? "INR" : job.Currency,
                Experience = !string.IsNullOrWhiteSpace(job.Experience) ? job.Experience : "1-3 Years",
                JobType = job.JobType,
                WorkMode = !string.IsNullOrWhiteSpace(job.WorkMode) ? job.WorkMode : "Hybrid",
                Openings = !string.IsNullOrWhiteSpace(job.Openings) ? job.Openings : "01",
                KeySkills = skills,

                ApplicantCount = applicantCount,
                PostedOn = job.PostedOn,
                ExpiryDate = job.ExpiryDate,
                ClosesIn = closesIn,
                RemainingSeconds = remainingSeconds,
                HasApplied = hasApplied,

                JobDescription = !string.IsNullOrWhiteSpace(job.JobDescription)
                    ? job.JobDescription
                    : "We are seeking a talented professional to join our dynamic team and build next-generation applications.",
            };
        }
    }
}
