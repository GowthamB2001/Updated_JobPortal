using AutoMapper;
using JobPortal.Application.DTOs.Applications;
using JobPortal.Application.DTOs.JobRenewals;
using JobPortal.Application.DTOs.JobSeekers;
using JobPortal.Application.DTOs.Recruiters;
using JobPortal.Application.DTOs.Resumes;
using JobPortal.Application.DTOs.Skills;
using JobPortal.Application.Features.Jobs.Queries.GetJobById;
using JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter;
using JobPortal.Application.Features.RegisterJobSeeker.Queries.LoginJobSeeker;
using JobPortal.Domain.Domain.Entities;
using System;
using System.Linq;

namespace JobPortal.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recruiter, RecruiterDto>().ReverseMap();

            CreateMap<Skill, SkillDto>().ReverseMap();

            CreateMap<JobSeeker, JobSeekerDto>().ReverseMap();

            CreateMap<Job, JobDetailsDto>()
                .ForMember(d => d.Skills,
                    o => o.MapFrom(s => s.JobSkills.Select(x => x.SkillName).ToList()))
                .ForMember(d => d.RecruiterType,
                    o => o.MapFrom(s => s.Recruiter != null ? s.Recruiter.OrganizationType.ToString() : null));

            CreateMap<Job, JobDto>()
                .ForMember(d => d.Skills,
                    o => o.MapFrom(s => s.JobSkills.Select(x => x.SkillName).ToList()))
                .ForMember(d => d.Status,
                    o => o.MapFrom(s => s.IsDraft ? "Draft" : (s.ExpiryDate > DateTime.UtcNow ? "Active" : "Expired")))
                .ForMember(d => d.RecruiterType,
                    o => o.MapFrom(s => s.Recruiter != null ? s.Recruiter.OrganizationType.ToString() : null));

            CreateMap<Resume, ResumeDto>().ReverseMap();

            CreateMap<JobApplication, JobApplicationDto>().ReverseMap();

            CreateMap<JobRenewal, JobRenewalDto>().ReverseMap();
            CreateMap<JobSeekerProfile, JobSeekerProfileDto>().ReverseMap();
            CreateMap<JobSeekerProfile, JobSeekerLoginResponse>();
        }
    }
}
