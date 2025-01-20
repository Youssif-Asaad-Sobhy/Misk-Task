using System.Collections.Concurrent;

namespace Misk_Task.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, DateTime> _lastRequestTimes = new();
        private static readonly TimeSpan _requestInterval = TimeSpan.FromSeconds(1);

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (_lastRequestTimes.TryGetValue(ipAddress, out DateTime lastRequestTime))
            {
                var timeSinceLastRequest = DateTime.UtcNow - lastRequestTime;
                if (timeSinceLastRequest < _requestInterval)
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Too many requests. Please try again later.",
                        retryAfter = (_requestInterval - timeSinceLastRequest).TotalSeconds
                    });
                    return;
                }
            }

            _lastRequestTimes.AddOrUpdate(ipAddress, DateTime.UtcNow, (_, _) => DateTime.UtcNow);
            await _next(context);
        }
    }
}
