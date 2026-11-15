using JobPortal.Application.DTOs.Legal;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.AppInfo.Queries.GetPrivacyPolicy
{
    public class GetPrivacyPolicyQueryHandler : IRequestHandler<GetPrivacyPolicyQuery, PrivacyPolicyDto>
    {
        public Task<PrivacyPolicyDto> Handle(GetPrivacyPolicyQuery request, CancellationToken cancellationToken)
        {
            var policy = new PrivacyPolicyDto
            {
                Title = "Privacy Policy",
                Version = "1.0",
                EffectiveDate = "2026-11-01",
                LastUpdated = "2026-11-15",
                Summary = "This Privacy Policy describes how Job Portal collects, uses, processes, and protects your personal data when using our platform as a job seeker or employer.",
                ContactEmail = "privacy@jobportal.com",
                Sections = new List<PolicySectionDto>
                {
                    new PolicySectionDto
                    {
                        Order = 1,
                        Title = "1. Information We Collect",
                        Content = "We collect information you provide directly to us when creating a profile, uploading resumes, or applying for jobs.",
                        BulletPoints = new List<string>
                        {
                            "Personal Identifiers: Full name, email address, phone number, and location.",
                            "Professional Profile: Work experience, education history, key skills, designations, and certifications.",
                            "Uploaded Documents: Resumes, CVs, portfolio links, and profile photos.",
                            "Device & Usage Data: IP address, device type, operating system, and in-app interaction data."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 2,
                        Title = "2. How We Use Your Information",
                        Content = "We process your information to deliver and improve our job matching and application tracking services.",
                        BulletPoints = new List<string>
                        {
                            "Match your skills and role preferences with relevant active job listings.",
                            "Facilitate direct job applications and share your profile with hiring recruiters.",
                            "Send important notifications regarding interview requests, job expirations, and status updates.",
                            "Ensure platform security, prevent fraud, and verify recruiter identities."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 3,
                        Title = "3. Information Sharing & Disclosure",
                        Content = "We only share your information with third parties in the following circumstances:",
                        BulletPoints = new List<string>
                        {
                            "With Recruiters: When you apply for a job or bookmark a role, relevant profile data is shared with the recruiter.",
                            "Service Providers: Trusted cloud hosting, notification, and email delivery partners under strict confidentiality agreements.",
                            "Legal Compliance: When required by applicable law, court order, or governmental authorities."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 4,
                        Title = "4. Data Security & Retention",
                        Content = "We maintain industry-standard administrative, technical, and physical safeguards including encryption in transit (HTTPS/TLS) and at rest. We retain your profile data as long as your account remains active or as needed to provide our services.",
                        BulletPoints = new List<string>
                        {
                            "Encrypted passwords and JWT-based authentication.",
                            "Continuous monitoring and vulnerability assessments.",
                            "You may request deletion of your account and personal data at any time."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 5,
                        Title = "5. Your Rights and Choices",
                        Content = "Depending on your jurisdiction, you have specific rights regarding your personal information:",
                        BulletPoints = new List<string>
                        {
                            "Access & Portability: Review and export the personal data we store about you.",
                            "Correction: Update inaccurate or outdated profile information directly in your settings.",
                            "Deletion: Request complete removal of your job seeker profile and associated application history.",
                            "Notification Preferences: Opt in or out of marketing communications and push notifications."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 6,
                        Title = "6. Contact Us",
                        Content = "If you have questions, concerns, or requests regarding this Privacy Policy or data protection, please contact our Data Protection Officer at privacy@jobportal.com or via support@jobportal.com."
                    }
                }
            };

            return Task.FromResult(policy);
        }
    }
}
