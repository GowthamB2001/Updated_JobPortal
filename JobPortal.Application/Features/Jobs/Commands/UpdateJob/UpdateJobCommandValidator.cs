using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.UpdateJob
{
    public class UpdateJobValidator
            : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty();

            RuleFor(x => x.RecruiterId)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Company)
                .NotEmpty();

            RuleFor(x => x.Location)
                .NotEmpty();

            RuleFor(x => x.JobDescription)
                .NotEmpty();

            RuleFor(x => x.CompanyOverview)
                .NotEmpty();

            RuleFor(x => x.KeySkills)
                .NotEmpty();

            RuleFor(x => x.JobType)
                .IsInEnum();
        }
    }
}
