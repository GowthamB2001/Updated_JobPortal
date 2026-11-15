using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Commands.DeleteRecruiter
{
    public class DeleteRecruiterCommandHandler
        : IRequestHandler<DeleteRecruiterCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRecruiterCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteRecruiterCommand request,
            CancellationToken cancellationToken)
        {
            var recruiter =
                await _unitOfWork.Recruiters
                    .GetByIdAsync(request.Id);

            if (recruiter == null)
                return false;

            await _unitOfWork.Recruiters
                .DeleteAsync(recruiter);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
