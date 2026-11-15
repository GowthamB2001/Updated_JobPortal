using JobPortal.Application.DTOs.JobRenewals;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobRenewals.Commands.Queries.GetRenewalByJob
{
    public class GetRenewalByJobQuery
       : IRequest<JobRenewalDto>
    {
        public Guid JobId { get; set; }
    }
}
