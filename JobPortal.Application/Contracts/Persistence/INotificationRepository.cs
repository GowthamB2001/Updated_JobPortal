using JobPortal.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetByUserIdAsync(Guid userId);
        Task<int> GetUnreadCountByUserIdAsync(Guid userId);
        Task<int> GetTotalCountByUserIdAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId, Guid userId);
        Task MarkAllAsReadByUserIdAsync(Guid userId);
    }
}
