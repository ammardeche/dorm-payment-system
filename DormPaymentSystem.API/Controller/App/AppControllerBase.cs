using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller.AuthController
{
    [ApiController]
    [Route("api/app/[controller]")]
    [Authorize(Roles = "Student")]
    public abstract class AppControllerBase : ControllerBase
    {

        protected string GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in claims.");
            }
            return userId;

        }
    }
}