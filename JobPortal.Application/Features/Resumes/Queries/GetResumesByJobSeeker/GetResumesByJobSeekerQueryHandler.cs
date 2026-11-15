using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Resumes;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Resumes.Queries.GetResumesByJobSeeker
{
    public class GetResumesByJobSeekerQueryHandler
        : IRequestHandler<GetResumesByJobSeekerQuery,
            List<ResumeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetResumesByJobSeekerQueryHandler(
     IUnitOfWork unitOfWork,
     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ResumeDto>> Handle(
            GetResumesByJobSeekerQuery request,
            CancellationToken cancellationToken)
        {
            var resumes =
                await _unitOfWork.Resumes
                    .GetByJobSeekerIdAsync(
                        request.JobSeekerId);
            return _mapper.Map<List<ResumeDto>>(resumes);
            //return resumes.Select(x => new ResumeDto
            //{
            //    Id = x.Id,
            //    JobSeekerId = x.JobSeekerId,
            //    FileName = x.FileName,
            //    FilePath = x.FilePath,
            //    FileType = x.FileType,
            //    FileSize = x.FileSize,
            //    UploadedDate = x.UploadedDate
            //}).ToList();
        }
    }
}
