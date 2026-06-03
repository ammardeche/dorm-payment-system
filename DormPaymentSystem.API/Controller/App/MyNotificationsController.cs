using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.AuthController;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller.App
{
    [ApiController]

    public class MyNotificationsController : AppControllerBase
    {

        private readonly INotificationService _notificationService;

        public MyNotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // student reads their own notifications
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications(
            [FromQuery] bool? isRead,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = GetCurrentUserId();
            var (items, totalCount) = await _notificationService
                .GetStudentNotificationsAsync(userId, isRead, pageIndex, pageSize);

            var pagination = new Pagination(pageSize, pageIndex, totalCount);
            return Ok(new Response<IEnumerable<NotificationResponse>>(
                items.Select(n => new NotificationResponse(n)), pagination));
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetCurrentUserId();
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new Response<int>(count));
        }

        [HttpPost("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = GetCurrentUserId();
            await _notificationService.MarkAsReadAsync(id, userId);
            return Ok(new Response<string>("Notification marked as read."));
        }

        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetCurrentUserId();
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new Response<string>("All notifications marked as read."));
        }
    }

}
