using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Commands.RenewJob
{
    public class RenewJobCommand : IRequest<Unit>
    {
        public Guid JobId { get; set; }

        public Guid RecruiterId { get; set; }
    }
}
