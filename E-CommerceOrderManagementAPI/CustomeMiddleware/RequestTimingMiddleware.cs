using System.Diagnostics;

namespace E_CommerceOrderManagementAPI.CustomeMiddleware
{
    public class RequestTimingMiddleware
    {
        private RequestDelegate _next;
        private ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation
             (
                "HTTP {Method} {Path} returned {StatusCode} in {ElepsedMilliseconds} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
             );
        }
    }
}
