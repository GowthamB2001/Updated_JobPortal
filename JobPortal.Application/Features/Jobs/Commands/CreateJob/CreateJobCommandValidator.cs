using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.RecruiterId)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Company)
                .NotEmpty();

            RuleFor(x => x.Location)
                .NotEmpty();

            RuleFor(x => x.SalaryRange)
                .NotEmpty();

            RuleFor(x => x.Experience)
                .NotEmpty();

            RuleFor(x => x.JobDescription)
                .NotEmpty();

            RuleFor(x => x.CompanyOverview)
                .NotEmpty();

            RuleFor(x => x.Openings)
                .NotEmpty();

            RuleFor(x => x.WorkMode)
                .NotEmpty();

            RuleFor(x => x.JobType)
                .IsInEnum();

            RuleFor(x => x.KeySkills)
                .NotEmpty()
                .WithMessage("At least one skill is required.");
        }
    }
}
