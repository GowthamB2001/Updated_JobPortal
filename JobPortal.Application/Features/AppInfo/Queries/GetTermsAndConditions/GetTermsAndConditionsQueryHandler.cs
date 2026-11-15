using JobPortal.Application.DTOs.Legal;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.AppInfo.Queries.GetTermsAndConditions
{
    public class GetTermsAndConditionsQueryHandler : IRequestHandler<GetTermsAndConditionsQuery, TermsAndConditionsDto>
    {
        public Task<TermsAndConditionsDto> Handle(GetTermsAndConditionsQuery request, CancellationToken cancellationToken)
        {
            var terms = new TermsAndConditionsDto
            {
                Title = "Terms and Conditions",
                Version = "1.0",
                EffectiveDate = "2026-11-01",
                LastUpdated = "2026-11-15",
                Summary = "These Terms and Conditions govern your access to and use of Job Portal's mobile application, website, and services. By registering, creating a profile, or applying for jobs, you agree to these terms.",
                ContactEmail = "legal@jobportal.com",
                Sections = new List<PolicySectionDto>
                {
                    new PolicySectionDto
                    {
                        Order = 1,
                        Title = "1. Acceptance of Terms",
                        Content = "By creating an account, browsing job listings, or submitting applications through Job Portal, you signify that you have read, understood, and agreed to be bound by these Terms and Conditions."
                    },
                    new PolicySectionDto
                    {
                        Order = 2,
                        Title = "2. User Eligibility & Account Responsibilities",
                        Content = "You must be at least 18 years of age or the age of legal majority in your jurisdiction to create an account.",
                        BulletPoints = new List<string>
                        {
                            "You are responsible for safeguarding your login credentials and JWT tokens.",
                            "All information submitted in your profile, resume, and application must be accurate, truthful, and verifiable.",
                            "Creating multiple misleading or duplicate accounts is strictly prohibited."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 3,
                        Title = "3. Job Applications & Recruiter Interactions",
                        Content = "Job Portal operates as an intermediary connecting job seekers and employers.",
                        BulletPoints = new List<string>
                        {
                            "Job Portal does not guarantee employment, interviews, or hiring outcomes.",
                            "Recruiters are independent third parties responsible for their job descriptions, interview scheduling, and employment offers.",
                            "Job seekers agree to communicate professionally and attend scheduled interviews promptly."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 4,
                        Title = "4. Prohibited Conduct",
                        Content = "Users agree not to engage in any prohibited activities on the platform, including:",
                        BulletPoints = new List<string>
                        {
                            "Submitting false credentials, forged resumes, or impersonating any person or entity.",
                            "Scraping, crawling, or extracting platform data without explicit written consent.",
                            "Transmitting viruses, malware, or exploiting platform APIs.",
                            "Harassing, discriminating against, or spamming recruiters or applicants."
                        }
                    },
                    new PolicySectionDto
                    {
                        Order = 5,
                        Title = "5. Intellectual Property",
                        Content = "All trademarks, logos, UI designs, algorithms, and software comprising the Job Portal application are the exclusive property of Job Portal Inc. or its licensors. You retain ownership of your submitted resume and personal content."
                    },
                    new PolicySectionDto
                    {
                        Order = 6,
                        Title = "6. Limitation of Liability",
                        Content = "To the maximum extent permitted by applicable law, Job Portal shall not be liable for any indirect, incidental, special, consequential, or punitive damages arising out of your access to or inability to access the platform."
                    },
                    new PolicySectionDto
                    {
                        Order = 7,
                        Title = "7. Termination & Suspension",
                        Content = "We reserve the right to suspend or terminate accounts that violate these terms, engage in fraudulent behavior, or compromise platform integrity."
                    },
                    new PolicySectionDto
                    {
                        Order = 8,
                        Title = "8. Governing Law & Dispute Resolution",
                        Content = "These terms shall be governed by and construed in accordance with the laws of the jurisdiction in which Job Portal operates, without regard to conflict of law principles."
                    },
                    new PolicySectionDto
                    {
                        Order = 9,
                        Title = "9. Contact Information",
                        Content = "If you have questions regarding these Terms and Conditions, please contact us at legal@jobportal.com."
                    }
                }
            };

            return Task.FromResult(terms);
        }
    }
}
