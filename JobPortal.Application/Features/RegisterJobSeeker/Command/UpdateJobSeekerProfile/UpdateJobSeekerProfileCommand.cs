using JobPortal.Application.DTOs.JobSeekers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.RegisterJobSeeker.Command.UpdateJobSeekerProfile
{
    public class UpdateJobSeekerProfileCommand
        : UpdateJobSeekerProfileRequest, IRequest
    {
        public Guid Id { get; set; }
    }
}
