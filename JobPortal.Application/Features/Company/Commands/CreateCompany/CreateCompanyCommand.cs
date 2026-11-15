using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Company.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<Guid>
    {
        public string CompanyName { get; set; }

        public string? Website { get; set; }

        public string Address { get; set; }

        public string OfficialEmail { get; set; }

        public Guid RecruiterId { get; set; }
    }
}
