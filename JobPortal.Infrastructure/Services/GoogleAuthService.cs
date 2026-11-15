using Google.Apis.Auth;
using JobPortal.Application.Contracts.Infrastructure;
using JobPortal.Application.DTOs.Common;
using JobPortal.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Infrastructure.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleAuthSettings _settings;

        public GoogleAuthService(
            IOptions<GoogleAuthSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<GoogleUserInfo> VerifyGoogleTokenAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[]
                    {
                        _settings.ClientId
                    }
                });

            return new GoogleUserInfo
            {
                GoogleId = payload.Subject,
                Name = payload.Name,
                Email = payload.Email,
                Picture = payload.Picture
            };
        }
    }
}