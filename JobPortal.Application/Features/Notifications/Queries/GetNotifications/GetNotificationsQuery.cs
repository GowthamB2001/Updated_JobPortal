using JobPortal.Application.DTOs.Notifications;
using MediatR;
using System;
using System.Collections.Generic;

namespace JobPortal.Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQuery : IRequest<List<NotificationDto>>
    {
        public Guid UserId { get; set; }
    }
}
