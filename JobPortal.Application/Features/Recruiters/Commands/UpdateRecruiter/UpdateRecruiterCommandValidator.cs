using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Commands.UpdateRecruiter
{
    public class UpdateRecruiterCommandValidator
         : AbstractValidator<UpdateRecruiterCommand>
    {
        public UpdateRecruiterCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.CompanyName)
                .NotEmpty();

            RuleFor(x => x.CompanyWebsite)
                .NotEmpty();
        }
    }
}
