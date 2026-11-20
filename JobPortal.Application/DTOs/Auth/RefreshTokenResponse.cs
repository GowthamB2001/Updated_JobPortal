using System;

namespace JobPortal.Application.DTOs.Auth
{
    public class RefreshTokenResponse
    {
        public bool Success { get; set; } = true;

        public string Message { get; set; } = "Token refreshed successfully.";

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public string Role { get; set; } = string.Empty;
    }
}
