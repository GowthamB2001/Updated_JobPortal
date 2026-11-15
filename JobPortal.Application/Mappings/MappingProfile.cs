using AutoMapper;
using JobPortal.Application.DTOs.Applications;

using JobPortal.Application.Features.Jobs.Queries.GetJobsByRecruiter;
using JobPortal.Application.DTOs.JobRenewals;
using JobPortal.Application.DTOs.JobSeekers;
using JobPortal.Application.DTOs.Recruiters;
using JobPortal.Application.DTOs.Resumes;
using JobPortal.Application.DTOs.Skills;
using JobPortal.Application.Features.RegisterJobSeeker.Queries.LoginJobSeeker;
using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Mappings
{
    public  class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recruiter, RecruiterDto>().ReverseMap();

            CreateMap<Skill, SkillDto>().ReverseMap();

            CreateMap<JobSeeker, JobSeekerDto>().ReverseMap();

            CreateMap<Job, JobDto>()
                .ForMember(d => d.Skills,
                    o => o.MapFrom(s => s.JobSkills.Select(x => x.SkillName).ToList()))
                .ForMember(d => d.Status,
                    o => o.MapFrom(s => s.ExpiryDate > DateTime.UtcNow ? "Active" : "Expired"));
            CreateMap<Resume, ResumeDto>().ReverseMap();

            CreateMap<JobApplication, JobApplicationDto>().ReverseMap();

            CreateMap<JobRenewal, JobRenewalDto>().ReverseMap();
            CreateMap<JobSeekerProfile, JobSeekerProfileDto>().ReverseMap();
            CreateMap<JobSeekerProfile, JobSeekerLoginResponse>();
        }
    }
}







