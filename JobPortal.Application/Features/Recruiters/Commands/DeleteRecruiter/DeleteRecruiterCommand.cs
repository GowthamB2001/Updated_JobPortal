using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Commands.DeleteRecruiter
{
    public class DeleteRecruiterCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
