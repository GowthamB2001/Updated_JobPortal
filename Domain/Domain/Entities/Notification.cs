using JobPortal.Domain.Domain.Common;
using System;

namespace JobPortal.Domain.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Type { get; set; } = "General";

        public string? TargetUrl { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
