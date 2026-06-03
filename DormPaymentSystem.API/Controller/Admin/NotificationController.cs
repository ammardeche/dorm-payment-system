using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.Admin;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]

    public class NotificationController : AdminControllerBase
    {

        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // admin reads their own system alerts
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications(
            [FromQuery] bool? isRead,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var (items, totalCount) = await _notificationService
                .GetAdminNotificationsAsync(userId, isRead, pageIndex, pageSize);

            var pagination = new Pagination(pageSize, pageIndex, totalCount);
            return Ok(new Response<IEnumerable<NotificationResponse>>(
                items.Select(n => new NotificationResponse(n)), pagination));
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new Response<int>(count));
        }

        // send to one student
        [HttpPost("send/student/{recipientUserId}")]
        public async Task<IActionResult> SendToStudent(
            string recipientUserId,
            [FromBody] SendNotificationRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _notificationService.SendToStudentAsync(
                userId, recipientUserId, req.Title, req.Message, req.Type);
            return Ok(new Response<string>("Notification sent successfully."));
        }

        // send to all students in a room
        [HttpPost("send/room/{roomId}")]
        public async Task<IActionResult> SendToRoom(
            int roomId,
            [FromBody] SendNotificationRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _notificationService.SendToRoomAsync(
                userId, roomId, req.Title, req.Message, req.Type);
            return Ok(new Response<string>("Notification sent to room successfully."));
        }

        // send to all active students
        [HttpPost("send/all")]
        public async Task<IActionResult> SendToAll([FromBody] SendNotificationRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _notificationService.SendToAllStudentsAsync(
                userId, req.Title, req.Message, req.Type);
            return Ok(new Response<string>("Notification sent to all students."));
        }

        [HttpPost("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _notificationService.MarkAsReadAsync(id, userId);
            return Ok(new Response<string>("Notification marked as read."));
        }
    }

}
