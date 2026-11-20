using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public Guid RecruiterId { get; set; }

        public string RecruiterName { get; set; } = string.Empty;

        public string OfficialEmail { get; set; } = string.Empty;
    }
}
