using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace PcosAnalyzer.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Proceed to the next middleware/controller
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details for the developer
                _logger.LogError($"An unhandled exception occurred: {ex.Message}");

                // Return a clean JSON response to the client
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred on the server.",
                Detailed = exception.Message // You can omit this in Production for security
            };

            // Ensure JSON properties follow camelCase (e.g., statusCode instead of StatusCode)
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}