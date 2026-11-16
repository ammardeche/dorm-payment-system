using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DormPaymentSystem.API.Infrastructure;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.API.Middleware
{
    public class BuildingContextMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(
        HttpContext context,
        BuildingContext buildingContext,
        DormPaymentDbContext dbContext)
        {
            if (context.GetEndpoint()?.Metadata.GetMetadata<RequireBuildingAttribute>() is null)
            {
                await next(context);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? context.User.FindFirstValue("sub");

            if (context.User.Identity?.IsAuthenticated != true ||
                string.IsNullOrWhiteSpace(userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var header = context.Request.Headers["X-Building-Id"];

            if (header.Count != 1 ||
                !int.TryParse(header[0], out var buildingId) ||
                buildingId <= 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Send one valid X-Building-Id header.");
                return;
            }

            var hasMembership = await dbContext.BuildingMemberships.AnyAsync(
                membership => membership.UserId == userId &&
                              membership.BuildingId == buildingId,
                context.RequestAborted);

            if (!hasMembership)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            buildingContext.BuildingId = buildingId;
            await next(context);
        }
    }
}