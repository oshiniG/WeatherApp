using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WeatherApp.Infrastructure.Middleware
{
    public class RequestFailureMiddleware
    {
        private static int _requestCount = 0;
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestFailureMiddleware> _logger;

        public RequestFailureMiddleware(RequestDelegate next, ILogger<RequestFailureMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _requestCount++;

            // Simulate 503 error every 5th request
            if (_requestCount % 5 == 0)
            {
                _logger.LogWarning("503 Service Unavailable triggered on request {RequestNumber}", _requestCount);
                context.Response.StatusCode = 503;
                await context.Response.WriteAsync("Service Unavailable");
                return;  
            }
           
            await _next(context);
        }
    }
}
