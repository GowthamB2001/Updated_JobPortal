using MediatR;
using System;

namespace JobPortal.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
