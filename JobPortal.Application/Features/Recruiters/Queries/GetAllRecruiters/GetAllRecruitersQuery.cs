using JobPortal.Application.DTOs.Recruiters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Queries.GetAllRecruiters
{
    public class GetAllRecruitersQuery
       : IRequest<List<RecruiterDto>>
    {
    }
}
