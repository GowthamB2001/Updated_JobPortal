using JobPortal.Application.Contracts.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JobPortal.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler
        : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            MarkNotificationAsReadCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.Notifications.MarkAsReadAsync(request.NotificationId, request.UserId);
            return true;
        }
    }
}
