using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Resumes.Commands.DeleteResume
{
    public class DeleteResumeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
