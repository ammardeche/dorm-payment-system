using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {



        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/notifications/unread
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var notifications = await _notificationService.GetUnreadNotificationsAsync();
            return Ok(notifications.Select(n => new NotificationResponse(n)));
        }

        // PUT: api/notifications/{id}/read
        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }

        // POST: api/notifications/send-daily
        // (This will be called by a scheduled job, not manually)
        [HttpPost("send-daily")]
        public async Task<IActionResult> SendDailyReminders()
        {
            await _notificationService.SendDailyPaymentRemindersAsync();
            return Ok("Daily reminders sent.");
        }
    }
}