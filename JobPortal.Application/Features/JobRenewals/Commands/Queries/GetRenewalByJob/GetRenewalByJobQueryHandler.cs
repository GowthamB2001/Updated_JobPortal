using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.JobRenewals;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobRenewals.Commands.Queries.GetRenewalByJob
{
    public class GetRenewalByJobQueryHandler
         : IRequestHandler<GetRenewalByJobQuery, JobRenewalDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRenewalByJobQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JobRenewalDto> Handle(
            GetRenewalByJobQuery request,
            CancellationToken cancellationToken)
        {
            var renewal =
                await _unitOfWork.JobRenewals
                    .GetByJobIdAsync(request.JobId);

            if (renewal == null)
                return null;

            return new JobRenewalDto
            {
                Id = renewal.Id,
                JobId = renewal.JobId,
                IsAutoRenewEnabled = renewal.IsAutoRenewEnabled,
                RenewalDays = renewal.RenewalDays,
                RenewalEndDate = renewal.RenewalEndDate,
                RenewedCount = renewal.RenewedCount
            };
        }
    }
}
