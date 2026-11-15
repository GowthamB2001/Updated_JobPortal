using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobRenewals.Commands.RenewJob
{
    public class RenewJobCommandValidator
        : AbstractValidator<RenewJobCommand>
    {
        public RenewJobCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty();

            RuleFor(x => x.RenewalDays)
                .GreaterThan(0);
        }
    }
}
