using FluentValidation;
using JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Command
{
    public class RegisterJobSeekerCommandValidator
        : AbstractValidator<RegisterJobSeekerCommand>
    {
        public RegisterJobSeekerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.MobileNo)
                .NotEmpty()
                .WithMessage("Mobile number is required.")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Mobile number must be 10 digits.");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Gender is required.");

            RuleFor(x => x.Education)
                .NotEmpty()
                .WithMessage("Education is required.");

            RuleFor(x => x.WorkExperience)
                .NotEmpty()
                .WithMessage("Work experience is required.");

            RuleFor(x => x.PreferredLocation)
                .NotEmpty()
                .WithMessage("Preferred location is required.");

            RuleFor(x => x.JobRole)
                .NotEmpty()
                .WithMessage("Job role is required.");

            When(x => x.WorkExperience == "Experienced", () =>
            {
                RuleFor(x => x)
                    .Must(x => x.YearsOfExperience.HasValue || x.TotalExperienceYears.HasValue || x.TotalExperienceMonths.HasValue)
                    .WithMessage("Years of experience is required.");

                RuleFor(x => x)
                    .Must(x => !string.IsNullOrWhiteSpace(x.LastRoleDesignation) || 
                               (x.Companies != null && x.Companies.Any(c => !string.IsNullOrWhiteSpace(c.Designation))) ||
                               !string.IsNullOrWhiteSpace(x.JobRole))
                    .WithMessage("Role or designation is required.");

                RuleFor(x => x)
                    .Must(x => x.LastMonthSalary.HasValue || x.LastCompanySalary.HasValue)
                    .WithMessage("Last company salary is required.");
            });

            When(x => x.WorkExperience == "Fresher", () =>
            {
                RuleFor(x => x.LookingFor)
                    .NotEmpty()
                    .WithMessage(
                        "Please select Fresher Job or Internship.");
            });
        }
    }
}