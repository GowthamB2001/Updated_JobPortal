using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.Features.Recruiters.Commands.CreateRecruits;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Recruiters.Commands.CreateRecruiters
{
    //public class CreateRecruiterCommandHandler
    //     : IRequestHandler<CreateRecruiterCommand, Guid>
    //{
    //    private readonly IUnitOfWork _unitOfWork;

    //    public CreateRecruiterCommandHandler(
    //        IUnitOfWork unitOfWork)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public async Task<Guid> Handle(
    //        CreateRecruiterCommand request,
    //        CancellationToken cancellationToken)
    //    {
    //        var recruiter = new Recruiter
    //        {
    //            UserId = request.UserId,
    //            CompanyName = request.CompanyName,
    //            CompanyWebsite = request.CompanyWebsite
    //        };

    //        await _unitOfWork.Recruiters
    //            .AddAsync(recruiter);

    //        await _unitOfWork.SaveChangesAsync();

    //        return recruiter.Id;
    //    }
    //}
}
