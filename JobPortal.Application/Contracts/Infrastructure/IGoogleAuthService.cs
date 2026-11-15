using JobPortal.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Infrastructure
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo> VerifyGoogleTokenAsync(string idToken);
    }
}
