using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Commands.UpdateRecruiter
{
    public class UpdateRecruiterCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyWebsite { get; set; }
    }
}
