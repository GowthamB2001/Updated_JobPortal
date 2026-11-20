using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Auth
{
    public class GoogleLoginResponse
    {
        public Guid GoogleLoginId { get; set; }

        public Guid JobSeekerId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public bool IsProfileCompleted { get; set; }
    }
}
