using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler
         : IRequestHandler<RegisterCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var users =
                await _unitOfWork.Users.GetAllAsync();

            if (users.Any(x => x.Email == request.Email))
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                    request.Password),
                Role = request.Role
            };

            await _unitOfWork.Users.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return user.Id;
        }
    }
}
