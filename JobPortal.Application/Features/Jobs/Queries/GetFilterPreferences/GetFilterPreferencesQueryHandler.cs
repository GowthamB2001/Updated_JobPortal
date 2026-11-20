using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Jobs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetFilterPreferences
{
    public class GetFilterPreferencesQueryHandler
        : IRequestHandler<GetFilterPreferencesQuery, FilterPreferencesDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        private static readonly List<string> PredefinedRoles = new()
        {
            // Frontend & Web
            "Frontend Developer",
            "React.js Developer",
            "Angular Developer",
            "Vue.js Developer",
            "UI/UX Designer",
            "UI Designer",
            "UX Designer",
            "UX Researcher",
            "Product Designer",
            "Graphic Designer",
            "Motion Graphics Designer",
            "Web Developer",
            "WordPress Developer",

            // Backend & Languages
            "Backend Developer",
            "Full Stack Developer",
            "Software Engineer",
            "Senior Software Engineer",
            "Lead Software Engineer",
            "Principal Software Engineer",
            "Java Developer",
            "Spring Boot Developer",
            ".NET Developer",
            "C# Developer",
            "Python Developer",
            "Django Developer",
            "FastAPI Developer",
            "Node.js Developer",
            "Golang Developer",
            "PHP Developer",
            "Laravel Developer",
            "Ruby on Rails Developer",
            "Rust Developer",
            "C++ Developer",
            "C Developer",
            "Embedded Software Engineer",
            "Firmware Engineer",

            // Mobile
            "Mobile App Developer",
            "Android Developer",
            "iOS Developer",
            "Flutter Developer",
            "React Native Developer",
            "Swift Developer",
            "Kotlin Developer",

            // Cloud, DevOps & Infrastructure
            "DevOps Engineer",
            "Cloud Engineer",
            "Site Reliability Engineer (SRE)",
            "AWS Solutions Architect",
            "Azure Cloud Engineer",
            "GCP Cloud Engineer",
            "Kubernetes Specialist",
            "Build & Release Engineer",
            "System Administrator",
            "Linux Administrator",
            "Windows Server Administrator",
            "Network Engineer",
            "Network Security Engineer",

            // Data, AI & Machine Learning
            "Data Scientist",
            "Data Analyst",
            "Data Engineer",
            "Big Data Engineer",
            "Machine Learning Engineer",
            "AI Engineer",
            "Deep Learning Specialist",
            "Generative AI Specialist",
            "NLP Engineer",
            "Computer Vision Engineer",
            "MLOps Engineer",
            "Business Intelligence (BI) Developer",
            "Power BI Developer",
            "Tableau Developer",
            "Database Administrator (DBA)",
            "Database Developer",
            "SQL Developer",

            // QA & Testing
            "QA Engineer",
            "Automation Test Engineer",
            "Manual Test Engineer",
            "SDET (Software Development Engineer in Test)",
            "Performance Test Engineer",
            "Security Test Engineer",
            "Mobile QA Engineer",

            // Cybersecurity
            "Cybersecurity Analyst",
            "Information Security Specialist",
            "SOC Analyst",
            "Penetration Tester / Ethical Hacker",
            "Cloud Security Engineer",
            "Application Security (AppSec) Engineer",
            "Security Architect",

            // Product, Project & Agile Management
            "Product Manager",
            "Associate Product Manager",
            "Technical Product Manager",
            "Product Owner",
            "Project Manager",
            "Technical Program Manager (TPM)",
            "Scrum Master",
            "Agile Coach",
            "Business Analyst",
            "IT Project Lead",
            "Engineering Manager",
            "Technical Lead",
            "Solutions Architect",
            "Enterprise Architect",

            // Specialized & Emerging Tech
            "Blockchain Developer",
            "Smart Contract Engineer",
            "Web3 Developer",
            "Game Developer",
            "Unity Developer",
            "Unreal Engine Developer",
            "AR/VR Developer",
            "IoT Engineer",
            "Robotics Engineer",
            "RPA Developer (UiPath / Automation Anywhere)",
            "Salesforce Developer",
            "Salesforce Administrator",
            "SAP Consultant",
            "ServiceNow Developer",
            "Workday Consultant",

            // Operations, Support & Business
            "Technical Writer",
            "IT Support Specialist",
            "Help Desk Engineer",
            "Digital Marketing Specialist",
            "SEO Specialist",
            "Growth Marketing Manager",
            "Content Strategist",
            "HR Recruiter",
            "Technical Recruiter",
            "Talent Acquisition Specialist",
            "Customer Success Manager",
            "Operations Manager"
        };

        private static readonly List<string> PredefinedExperiences = new()
        {
            "Entry Level",
            "1-3 Years",
            "3-5 Years",
            "5-10 Years",
            "10+ Years"
        };

        private static readonly List<string> PredefinedJobTypes = new()
        {
            "Full Time",
            "Part Time",
            "Contract",
            "Internship",
            "Freelance"
        };

        private static readonly List<string> PredefinedWorkModes = new()
        {
            "Remote",
            "Hybrid",
            "On-site"
        };

        private static readonly List<string> PredefinedLocations = new()
        {
            "Bengaluru",
            "Hyderabad",
            "Chennai",
            "Pune",
            "Mumbai",
            "Delhi NCR",
            "Noida",
            "Gurugram",
            "Kolkata",
            "Ahmedabad",
            "Coimbatore",
            "Kochi",
            "Chandigarh",
            "Jaipur",
            "Indore"
        };

        private static readonly List<string> PredefinedSkills = new()
        {
            "Java", "Python", "JavaScript", "TypeScript", "React.js", "Angular",
            "Node.js", ".NET", "C#", "Flutter", "Dart", "AWS", "Azure",
            "Docker", "Kubernetes", "SQL", "MongoDB", "Figma", "UI/UX",
            "Machine Learning", "DevOps", "REST API", "Git", "HTML/CSS",
            "Spring Boot", "C++", "Linux", "CI/CD", "Tailwind CSS", "Next.js"
        };

        public GetFilterPreferencesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FilterPreferencesDto> Handle(
            GetFilterPreferencesQuery request,
            CancellationToken cancellationToken)
        {
            var rolesSet = new HashSet<string>(PredefinedRoles, StringComparer.OrdinalIgnoreCase);
            var locationsSet = new HashSet<string>(PredefinedLocations, StringComparer.OrdinalIgnoreCase);
            var skillsSet = new HashSet<string>(PredefinedSkills, StringComparer.OrdinalIgnoreCase);

            try
            {
                var activeJobs = await _unitOfWork.Jobs.GetAllJobsAsync();
                foreach (var job in activeJobs)
                {
                    if (!string.IsNullOrWhiteSpace(job.Title))
                    {
                        rolesSet.Add(job.Title.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(job.Location))
                    {
                        locationsSet.Add(job.Location.Trim());
                    }

                    if (job.JobSkills != null)
                    {
                        foreach (var s in job.JobSkills)
                        {
                            if (!string.IsNullOrWhiteSpace(s.SkillName))
                            {
                                skillsSet.Add(s.SkillName.Trim());
                            }
                        }
                    }
                }
            }
            catch
            {
                // Fallback gracefully to predefined lists
            }

            return new FilterPreferencesDto
            {
                SalaryRange = new SalaryRangeFilterDto
                {
                    MinLpa = 0,
                    MaxLpa = 50,
                    Currency = "INR",
                    CurrencySymbol = "₹",
                    Unit = "LPA",
                    MinLabel = "₹0",
                    MaxLabel = "₹50L+"
                },
                Roles = rolesSet.OrderBy(r => r).ToList(),
                ExperienceLevels = PredefinedExperiences,
                JobTypes = PredefinedJobTypes,
                WorkModes = PredefinedWorkModes,
                PopularLocations = locationsSet.Take(30).ToList(),
                PopularSkills = skillsSet.Take(30).ToList()
            };
        }
    }
}
