using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.AuthController;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller.App
{

    public class MyProfileController : AppControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IAuthService _authService;

        public MyProfileController(
            IStudentService studentService,
            IAuthService authService)
        {
            _studentService = studentService;
            _authService = authService;
        }

        // GET api/app/myprofile
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var studentId = GetCurrentStudentId();
            var student = await _studentService.GetStudentByIdAsync(studentId);
            return Ok(new Response<StudentResponse>(new StudentResponse(student)));
        }


        // POST api/app/myprofile/change-password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordRequest req)
        {
            var userId = GetCurrentUserId();
            await _authService.ChangePasswordAsync(
                userId, req.CurrentPassword, req.NewPassword);
            return Ok(new Response<string>("Password changed successfully."));
        }

        // POST api/app/myprofile/fcm-token
        // mobile app calls this after login to enable push notifications
        // [HttpPost("fcm-token")]
        // public async Task<IActionResult> UpdateFcmToken(
        //     [FromBody] UpdateFcmTokenRequest req)
        // {
        //     var userId = GetCurrentUserId();
        //     await _authService.UpdateFcmTokenAsync(userId, req.FcmToken);
        //     return Ok(new Response<string>("Device token updated."));
        // }
    }
}
