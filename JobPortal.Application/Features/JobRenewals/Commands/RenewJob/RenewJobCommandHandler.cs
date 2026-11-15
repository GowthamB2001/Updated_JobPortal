using JobPortal.Application.Contracts.Persistence;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.JobRenewals.Commands.RenewJob
{
    public class RenewJobCommandHandler
         : IRequestHandler<RenewJobCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RenewJobCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            RenewJobCommand request,
            CancellationToken cancellationToken)
        {
            var existingRenewal =
                await _unitOfWork.JobRenewals
                    .GetByJobIdAsync(request.JobId);

            if (existingRenewal == null)
            {
                var renewal = new JobRenewal
                {
                    JobId = request.JobId,
                    IsAutoRenewEnabled = true,
                    RenewalDays = request.RenewalDays,
                    RenewalEndDate = DateTime.UtcNow.AddDays(request.RenewalDays),
                    RenewedCount = 1
                };

                await _unitOfWork.JobRenewals
                    .AddAsync(renewal);

                await _unitOfWork.SaveChangesAsync();

                return renewal.Id;
            }

            existingRenewal.RenewalDays = request.RenewalDays;
            existingRenewal.RenewalEndDate =
                existingRenewal.RenewalEndDate.AddDays(request.RenewalDays);

            existingRenewal.RenewedCount++;

            await _unitOfWork.JobRenewals
                .UpdateAsync(existingRenewal);

            await _unitOfWork.SaveChangesAsync();

            return existingRenewal.Id;
        }
    }
}
