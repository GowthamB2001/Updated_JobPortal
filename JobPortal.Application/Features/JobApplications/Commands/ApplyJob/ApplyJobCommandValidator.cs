using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Commands.ApplyJob
{
    public class ApplyJobCommandValidator
           : AbstractValidator<ApplyJobCommand>
    {
        public ApplyJobCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty();

            RuleFor(x => x.JobSeekerId)
                .NotEmpty();
        }
    }
}
