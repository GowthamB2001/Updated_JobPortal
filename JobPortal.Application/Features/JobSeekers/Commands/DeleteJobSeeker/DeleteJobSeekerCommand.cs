using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Commands.DeleteJobSeeker
{
    public class DeleteJobSeekerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
