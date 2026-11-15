using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.RenewJob
{
    public class RenewJobValidator
        : AbstractValidator<RenewJobCommand>
    {
        public RenewJobValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty();

            RuleFor(x => x.RecruiterId)
                .NotEmpty();
        }
    }
}
