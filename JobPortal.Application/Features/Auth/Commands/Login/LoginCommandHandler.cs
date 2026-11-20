using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Auth;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler
           : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var recruiter = await _unitOfWork.Recruiters
                .GetByOfficialEmailAsync(request.OfficialEmail);

            if (recruiter == null)
                throw new Exception("Invalid Email");

            bool validPassword = BCrypt.Net.BCrypt.Verify(
                request.Password,
                recruiter.PasswordHash);

            if (!validPassword)
                throw new Exception("Invalid Password");

            var token = _jwtTokenGenerator.GenerateToken(recruiter);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            return new LoginResponse
            {
                Success = true,
                Message = "Login Successful",
                Token = token,
                RefreshToken = refreshToken,
                RecruiterId= recruiter.Id,
                RecruiterName = recruiter.RecruiterName,
                OfficialEmail = recruiter.OfficialEmail
            };
        }
    }
}
