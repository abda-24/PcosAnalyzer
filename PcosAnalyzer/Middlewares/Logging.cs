using System.Diagnostics;
using System.Text;

namespace PcosAnalyzer.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           
            var timer = Stopwatch.StartNew();

            try
            {
               
                _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

                
                await _next(context);

               
                timer.Stop();
                var elapsedMs = timer.ElapsedMilliseconds;

                
                _logger.LogInformation($"Finished Request: {context.Response.StatusCode} in {elapsedMs}ms");
            }
            catch (Exception)
            {
                timer.Stop();
                throw; 
            }
        }
    }
}