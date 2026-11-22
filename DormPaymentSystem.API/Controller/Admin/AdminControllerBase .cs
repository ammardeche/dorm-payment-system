using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.API.Controller.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin,Manager,Receptionist")]
    public abstract class AdminControllerBase : ControllerBase
    {

        protected string GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("User is not authenticated.");
            return userId;
        }

        // reads studentId directly from JWT claim — no DB query needed
        protected int GetCurrentStudentId()
        {
            var studentIdClaim = User.FindFirstValue("studentId");
            if (string.IsNullOrEmpty(studentIdClaim))
                throw new UnauthorizedException("Student identity not found in token.");
            return int.Parse(studentIdClaim);
        }

    }
}