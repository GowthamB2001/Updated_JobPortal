using System;
using System.Collections.Generic;

namespace JobPortal.Application.DTOs.Legal
{
    public class AppInfoDto
    {
        public string AppName { get; set; } = "Job Portal";
        public string Version { get; set; } = "1.0.0";
        public string BuildNumber { get; set; } = "100";
        public string Tagline { get; set; } = "Find your dream job faster with smart recommendations.";
        public string Description { get; set; } = "Job Portal connects job seekers with verified employers and recruiters worldwide. Explore personalized job matches, track applications in real time, and communicate directly with hiring teams.";
        public List<string> KeyFeatures { get; set; } = new()
        {
            "Smart AI-based Job Recommendations & Skill Matching",
            "Real-time Application Status Tracking",
            "Instant Alerts for Expiring & Closing Jobs",
            "Bookmark and Save Jobs for Later",
            "Direct Interview Scheduling & Video Calls",
            "Comprehensive Recruiter Verification"
        };
        public string SupportEmail { get; set; } = "support@jobportal.com";
        public string WebsiteUrl { get; set; } = "https://jobportal.com";
        public string HelpCenterUrl { get; set; } = "https://jobportal.com/help";
        public string PrivacyPolicyUrl { get; set; } = "/api/AppInfo/privacy-policy";
        public string TermsAndConditionsUrl { get; set; } = "/api/AppInfo/terms-and-conditions";
        public string Copyright { get; set; } = $"© {DateTime.UtcNow.Year} Job Portal Inc. All rights reserved.";
    }

    public class PolicySectionDto
    {
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string>? BulletPoints { get; set; }
    }

    public class PrivacyPolicyDto
    {
        public string Title { get; set; } = "Privacy Policy";
        public string Version { get; set; } = "1.0";
        public string EffectiveDate { get; set; } = "2026-11-01";
        public string LastUpdated { get; set; } = "2026-11-15";
        public string Summary { get; set; } = "This Privacy Policy describes how Job Portal collects, uses, and safeguards your personal data when you use our job-seeking platform.";
        public List<PolicySectionDto> Sections { get; set; } = new();
        public string ContactEmail { get; set; } = "privacy@jobportal.com";
    }

    public class TermsAndConditionsDto
    {
        public string Title { get; set; } = "Terms and Conditions";
        public string Version { get; set; } = "1.0";
        public string EffectiveDate { get; set; } = "2026-11-01";
        public string LastUpdated { get; set; } = "2026-11-15";
        public string Summary { get; set; } = "Please read these Terms and Conditions carefully before using our mobile application or website. By accessing or using Job Portal, you agree to be bound by these terms.";
        public List<PolicySectionDto> Sections { get; set; } = new();
        public string ContactEmail { get; set; } = "legal@jobportal.com";
    }
}
