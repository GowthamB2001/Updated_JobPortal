using JobPortal.Application.DTOs.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobApplications.Queries.GetApplicationById
{
    public class GetApplicationByIdQuery
         : IRequest<JobApplicationDto>
    {
        public Guid Id { get; set; }
    }
}
