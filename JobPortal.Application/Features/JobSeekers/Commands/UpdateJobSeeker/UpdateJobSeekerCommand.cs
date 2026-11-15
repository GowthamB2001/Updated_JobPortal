using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Commands.UpdateJobSeeker
{
    public class UpdateJobSeekerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Location { get; set; }
    }
}
