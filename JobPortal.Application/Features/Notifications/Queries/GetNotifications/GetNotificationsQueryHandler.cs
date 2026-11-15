using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Notifications;
using JobPortal.Domain.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQueryHandler
        : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<NotificationDto>> Handle(
            GetNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var notifications = (await _unitOfWork.Notifications.GetByUserIdAsync(request.UserId)).ToList();

            // Auto-populate initial notifications from recruiter's job applications if none exist yet
            if (!notifications.Any())
            {
                var allJobs = await _unitOfWork.Jobs.GetAllAsync();
                var recruiterJobIds = allJobs
                    .Where(j => j.RecruiterId == request.UserId)
                    .Select(j => j.Id)
                    .ToHashSet();

                if (recruiterJobIds.Any())
                {
                    var allApplications = await _unitOfWork.JobApplications.GetAllAsync();
                    var recruiterApps = allApplications
                        .Where(a => recruiterJobIds.Contains(a.JobId))
                        .OrderByDescending(a => a.CreatedOn)
                        .Take(5)
                        .ToList();

                    foreach (var app in recruiterApps)
                    {
                        var job = allJobs.FirstOrDefault(j => j.Id == app.JobId);
                        var newNotification = new Notification
                        {
                            Id = Guid.NewGuid(),
                            UserId = request.UserId,
                            Title = "New Job Application Received",
                            Message = $"A candidate applied for your job '{job?.Title ?? "Open Role"}'.",
                            Type = "Application",
                            TargetUrl = $"/applications/{app.Id}",
                            IsRead = false,
                            CreatedOn = app.CreatedOn,
                            ModifiedOn = app.CreatedOn,
                            IsDeleted = false
                        };

                        await _unitOfWork.Notifications.AddAsync(newNotification);
                        notifications.Add(newNotification);
                    }
                }

                // If still empty, add a welcome notification
                if (!notifications.Any())
                {
                    var welcomeNotification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        UserId = request.UserId,
                        Title = "Welcome to JobPortal!",
                        Message = "Your recruiter account is set up and active. Start posting jobs to receive candidate applications.",
                        Type = "System",
                        TargetUrl = "/jobs/create",
                        IsRead = false,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _unitOfWork.Notifications.AddAsync(welcomeNotification);
                    notifications.Add(welcomeNotification);
                }

                await _unitOfWork.SaveChangesAsync();
            }

            return notifications
                .OrderByDescending(n => n.CreatedOn)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type,
                    TargetUrl = n.TargetUrl,
                    IsRead = n.IsRead,
                    CreatedOn = n.CreatedOn,
                    TimeAgo = GetTimeAgo(n.CreatedOn)
                })
                .ToList();
        }

        private static string GetTimeAgo(DateTime dateTime)
        {
            var span = DateTime.UtcNow - dateTime;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            if (span.TotalDays < 7) return $"{(int)span.TotalDays}d ago";
            return dateTime.ToString("dd MMM yyyy");
        }
    }
}
