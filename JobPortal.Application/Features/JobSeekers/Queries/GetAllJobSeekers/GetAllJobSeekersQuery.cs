using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobSeekers.Queries.GetAllJobSeekers
{
    public class GetAllJobSeekersQuery
        : IRequest<List<JobSeekerDto>>
    {
    }
}
