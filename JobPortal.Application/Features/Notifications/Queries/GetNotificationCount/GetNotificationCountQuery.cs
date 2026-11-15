using JobPortal.Application.DTOs.Notifications;
using MediatR;
using System;

namespace JobPortal.Application.Features.Notifications.Queries.GetNotificationCount
{
    public class GetNotificationCountQuery : IRequest<NotificationCountDto>
    {
        public Guid UserId { get; set; }
    }
}
