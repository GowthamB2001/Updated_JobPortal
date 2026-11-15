using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobRenewals.Commands.RenewJob
{
    public class RenewJobCommand : IRequest<Guid>
    {
        public Guid JobId { get; set; }

        public int RenewalDays { get; set; }
    }
}
