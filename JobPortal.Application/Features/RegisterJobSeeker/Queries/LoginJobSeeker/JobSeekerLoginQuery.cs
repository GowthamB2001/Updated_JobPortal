using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Queries.LoginJobSeeker
{
    public class JobSeekerLoginQuery : IRequest<JobSeekerLoginResponse?>
    {
        public string MobileNo { get; set; } = string.Empty;
    }
}
