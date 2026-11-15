using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Interviews.Commands.AcceptInterview
{
    public class AcceptInterviewCommandHandler
        : IRequestHandler<AcceptInterviewCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AcceptInterviewCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            AcceptInterviewCommand request,
            CancellationToken cancellationToken)
        {
            var interview =
                await _unitOfWork.Interviews
                    .GetByIdAsync(request.InterviewId);

            if (interview == null)
                throw new Exception("Interview not found");

            interview.Status =
                InterviewStatus.Accepted;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
