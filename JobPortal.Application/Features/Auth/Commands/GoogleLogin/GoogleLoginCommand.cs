using JobPortal.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.GoogleLogin
{
    public class GoogleLoginCommand : IRequest<GoogleLoginResponse>
    {
        public string IdToken { get; set; } = string.Empty;
    }
}
