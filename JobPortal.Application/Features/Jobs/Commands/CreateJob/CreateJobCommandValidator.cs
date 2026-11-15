using FluentValidation;

namespace JobPortal.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.RecruiterId)
                .NotEmpty().WithMessage("Recruiter ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job Title is required.")
                .MaximumLength(150);

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Job Location is required.");

            RuleFor(x => x.SalaryRange)
                .NotEmpty()
                .When(x => !x.MinSalary.HasValue && !x.MaxSalary.HasValue)
                .WithMessage("Salary Range or Minimum/Maximum Salary is required.");

            RuleFor(x => x.Experience)
                .NotEmpty().WithMessage("Experience is required.");

            RuleFor(x => x.JobDescription)
                .NotEmpty().WithMessage("Job Description is required.");

            RuleFor(x => x.Openings)
                .NotEmpty().WithMessage("Openings count is required.");

            RuleFor(x => x.WorkMode)
                .NotEmpty().WithMessage("Work Mode is required.");

            RuleFor(x => x.JobType)
                .IsInEnum().WithMessage("A valid Job Type must be selected.");

            RuleFor(x => x.KeySkills)
                .NotEmpty()
                .WithMessage("At least one key skill is required.");
        }
    }
}
