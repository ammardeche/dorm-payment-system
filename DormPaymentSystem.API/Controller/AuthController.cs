using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // admin + receptionist login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _authService.LoginAsync(req.Email, req.Password);
            return Ok(new Response<LoginResponse>(new LoginResponse
            {
                Token = result.Token,
                Email = result.User.Email!,
                FullName = result.User.FullName,
                Role = result.Role
            }));
        }

        // student mobile app login
        [HttpPost("student-login")]
        [AllowAnonymous]
        public async Task<IActionResult> StudentLogin(
            [FromBody] StudentLoginRequest req)
        {
            var result = await _authService.StudentLoginAsync(
                req.StudentNumber, req.Password);

            return Ok(new Response<StudentLoginResponse>(new StudentLoginResponse
            {
                Token = result.Token,
                FullName = result.User.FullName,
                StudentNumber = req.StudentNumber,
                StudentId = result.StudentId!.Value,
                Role = result.Role
            }));
        }

        // admin registers staff — protected, Admin only
        [HttpPost("register-staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterStaff(
            [FromBody] RegisterStaffRequest req)
        {
            var user = await _authService.RegisterStaffAsync(
                req.FirstName, req.LastName,
                req.Email, req.Password, req.Role);

            return Ok(new Response<string>($"Staff account created for {user.FullName}."));
        }

        // student changes their own password from mobile app
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _authService.ChangePasswordAsync(
                userId, req.CurrentPassword, req.NewPassword);

            return Ok(new Response<string>("Password changed successfully."));
        }

        // student registers FCM token after login — mobile app calls this
        [HttpPost("fcm-token")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateFcmToken(
            [FromBody] UpdateFcmTokenRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _authService.UpdateFcmTokenAsync(userId, req.FcmToken);
            return Ok(new Response<string>("Device token updated."));
        }
    }

}
