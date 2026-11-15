using AutoMapper;
using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Queries.GetJobSeekerProfileById
{
    public class GetJobSeekerProfileByIdQueryHandler
        : IRequestHandler<GetJobSeekerProfileByIdQuery, JobSeekerProfileDto?>
    {
        private readonly IJobSeekerProfileRepository _repository;
        private readonly IMapper _mapper;

        public GetJobSeekerProfileByIdQueryHandler(
            IJobSeekerProfileRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<JobSeekerProfileDto?> Handle(
            GetJobSeekerProfileByIdQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.Id);

            if (profile == null)
                return null;

            var dto = _mapper.Map<JobSeekerProfileDto>(profile);

            // Ensure Companies list is populated from CompaniesWorkedJson if not already set
            if ((dto.Companies == null || !dto.Companies.Any()) && !string.IsNullOrWhiteSpace(profile.CompaniesWorkedJson))
            {
                try
                {
                    dto.Companies = System.Text.Json.JsonSerializer.Deserialize<List<CompanyExperienceDto>>(
                        profile.CompaniesWorkedJson,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<CompanyExperienceDto>();
                }
                catch
                {
                    dto.Companies = new List<CompanyExperienceDto>();
                }
            }

            // Fallback for experience years/months if only YearsOfExperience was populated
            if (!dto.TotalExperienceYears.HasValue && !dto.TotalExperienceMonths.HasValue && dto.YearsOfExperience.HasValue)
            {
                var totalYears = dto.YearsOfExperience.Value;
                dto.TotalExperienceYears = (int)Math.Truncate(totalYears);
                dto.TotalExperienceMonths = (int)Math.Round((totalYears - Math.Truncate(totalYears)) * 12);
            }

            // Fallback for annual salary if only LastMonthSalary was populated
            if (!dto.LastCompanySalary.HasValue && dto.LastMonthSalary.HasValue)
            {
                dto.LastCompanySalary = dto.LastMonthSalary.Value * 12m;
            }

            // Fallback for current location if empty
            if (string.IsNullOrWhiteSpace(dto.CurrentLocation) && !string.IsNullOrWhiteSpace(dto.PreferredLocation))
            {
                dto.CurrentLocation = dto.PreferredLocation;
            }

            return dto;
        }
    }
}
