using FluentValidation;
using JobPortal.Application.Features.Recruiters.Commands.CreateRecruits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Commands.CreateRecruiters
{
    public class CreateRecruiterCommandValidator
         : AbstractValidator<CreateRecruiterCommand>
    {
        public CreateRecruiterCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.CompanyWebsite)
                .NotEmpty();
        }
    }
}
