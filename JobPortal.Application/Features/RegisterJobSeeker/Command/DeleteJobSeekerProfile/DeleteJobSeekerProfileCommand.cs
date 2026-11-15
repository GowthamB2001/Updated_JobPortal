using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Command.DeleteJobSeekerProfile
{
    public class DeleteJobSeekerProfileCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
