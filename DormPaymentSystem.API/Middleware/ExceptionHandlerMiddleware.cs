using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var traceId = Activity.Current?.TraceId.ToString()
                    ?? context.TraceIdentifier;

                var appException = ex as AppException;
                var statusCode = appException?.StatusCode
                    ?? StatusCodes.Status500InternalServerError;

                var message = appException?.Message
                    ?? "An unexpected error occurred. Please try again later.";

                if (context.Response.HasStarted)
                {
                    _logger.LogError(
                        ex,
                        "Request failed after the response started. TraceId={TraceId}",
                        traceId);

                    throw;
                }

                if (statusCode >= 500)
                {
                    _logger.LogError(
                        ex,
                        "Request failed. TraceId={TraceId} Method={Method} StatusCode={StatusCode}",
                        traceId, context.Request.Method, statusCode);
                }
                else
                {
                    _logger.LogInformation(
                        "Request rejected. TraceId={TraceId} Method={Method} StatusCode={StatusCode} ExceptionType={ExceptionType}",
                        traceId, context.Request.Method, statusCode, ex.GetType().Name);
                }

                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    statusCode,
                    message,
                    traceId
                });
            }
        }


    }
}