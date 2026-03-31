using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Exceptions;

namespace DormPaymentSystem.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlerMiddleware> _logger;


        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught {Message}", ex.Message);
                await HandleExceptionAsync(context, ex); //  THIS WAS MISSING
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message;

            if (exception is AppException appEx)
            {
                // All your custom exceptions (NotFound, Conflict, Validation...)
                // automatically carry their own status code — no switch needed
                statusCode = appEx.StatusCode;
                message = appEx.Message;
            }
            else
            {
                // Anything unexpected (DB crash, null ref, etc.)
                statusCode = 500;
                message = "An unexpected error occurred. Please try again later.";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                success = false,
                statusCode,
                message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
            );
        }



    }
}