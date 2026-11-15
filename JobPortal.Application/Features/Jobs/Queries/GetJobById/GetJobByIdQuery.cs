using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Jobs.Queries.GetJobById
{
    public class GetJobByIdQuery : IRequest<JobDetailsDto>
    {
        public Guid Id { get; set; }
    }
}
