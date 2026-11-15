using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Commands.CreateJobSeeker
{
    public class CreateJobSeekerCommandValidator
        : AbstractValidator<CreateJobSeekerCommand>
    {
        public CreateJobSeekerCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
