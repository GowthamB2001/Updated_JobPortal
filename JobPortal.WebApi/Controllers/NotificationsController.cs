using JobPortal.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;
using JobPortal.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using JobPortal.Application.Features.Notifications.Queries.GetNotificationCount;
using JobPortal.Application.Features.Notifications.Queries.GetNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortal.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (Guid.TryParse(claim, out var id))
                return id;

            return null;
        }

        // 1. Notification Count API
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            var result = await _mediator.Send(new GetNotificationCountQuery
            {
                UserId = userId.Value
            });

            return Ok(result);
        }

        // 2. Notification List API
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            var result = await _mediator.Send(new GetNotificationsQuery
            {
                UserId = userId.Value
            });

            return Ok(result);
        }

        // 3. Mark Single Notification As Read API
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            await _mediator.Send(new MarkNotificationAsReadCommand
            {
                NotificationId = id,
                UserId = userId.Value
            });

            return Ok(new
            {
                Success = true,
                Message = "Notification marked as read."
            });
        }

        // 4. Mark All Notifications As Read API
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            await _mediator.Send(new MarkAllNotificationsAsReadCommand
            {
                UserId = userId.Value
            });

            return Ok(new
            {
                Success = true,
                Message = "All notifications marked as read."
            });
        }
    }
}
