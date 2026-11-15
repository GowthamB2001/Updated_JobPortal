using JobPortal.Application.Contracts.Persistence;
using JobPortal.Application.DTOs.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Notifications.Queries.GetNotificationCount
{
    public class GetNotificationCountQueryHandler
        : IRequestHandler<GetNotificationCountQuery, NotificationCountDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationCountDto> Handle(
            GetNotificationCountQuery request,
            CancellationToken cancellationToken)
        {
            var unread = await _unitOfWork.Notifications.GetUnreadCountByUserIdAsync(request.UserId);
            var total = await _unitOfWork.Notifications.GetTotalCountByUserIdAsync(request.UserId);

            return new NotificationCountDto
            {
                UnreadCount = unread,
                TotalCount = total
            };
        }
    }
}
