using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Commands.UpdateRecruiter
{
    //public class UpdateRecruiterCommandHandler
    //     : IRequestHandler<UpdateRecruiterCommand, bool>
    //{
    //    private readonly IUnitOfWork _unitOfWork;

    //    public UpdateRecruiterCommandHandler(
    //        IUnitOfWork unitOfWork)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public async Task<bool> Handle(
    //        UpdateRecruiterCommand request,
    //        CancellationToken cancellationToken)
    //    {
    //        var recruiter =
    //            await _unitOfWork.Recruiters
    //                .GetByIdAsync(request.Id);

    //        if (recruiter == null)
    //            return false;

    //        recruiter.UserId = request.UserId;
    //        recruiter.CompanyName = request.CompanyName;
    //        recruiter.CompanyWebsite = request.CompanyWebsite;

    //        await _unitOfWork.Recruiters
    //            .UpdateAsync(recruiter);

    //        await _unitOfWork.SaveChangesAsync();

    //        return true;
    //    }
    //}
}
