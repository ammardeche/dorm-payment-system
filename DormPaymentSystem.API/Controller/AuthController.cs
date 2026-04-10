using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
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

        // POST: api/auth/register
        // Only admin can create new receptionists
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.Register(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.ConfirmPassword,
                "Receptionist"
            );

            return CreatedAtAction(
                nameof(Register),
                new UserResponse(user)
            );
        }

        // POST: api/auth/login
        // Anyone can login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.Login(request.Email, request.Password);

            return Ok(new LoginResponse
            {
                Token = result.Token,
                Email = result.User.Email!,
                FullName = result.User.FullName,
                Role = result.Role
            });
        }
    }
}