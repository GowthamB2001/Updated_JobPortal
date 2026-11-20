using JobPortal.Application.DTOs.Auth;
using MediatR;
using System;

namespace JobPortal.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;

        public string? Token { get; set; }
    }
}
