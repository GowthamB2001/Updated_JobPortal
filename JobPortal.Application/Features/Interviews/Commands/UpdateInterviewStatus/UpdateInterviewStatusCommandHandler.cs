using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Commands.UpdateInterviewStatus
{
    public class UpdateInterviewStatusCommandHandler
         : IRequestHandler<
             UpdateInterviewStatusCommand,
             bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInterviewStatusCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateInterviewStatusCommand request,
            CancellationToken cancellationToken)
        {
            var interview =
                await _unitOfWork.Interviews
                    .GetByIdAsync(request.InterviewId);

            if (interview == null)
                throw new Exception("Interview not found");

            interview.Status = request.Status;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
