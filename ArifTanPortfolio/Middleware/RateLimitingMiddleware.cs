using System.Collections.Concurrent;
using System.Net;

namespace ArifTanPortfolio.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, RateLimitInfo> _clients = new();
        private readonly ILogger<RateLimitingMiddleware> _logger;
        
        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = GetClientKey(context);
            var rateLimitInfo = _clients.GetOrAdd(key, _ => new RateLimitInfo());

            bool isRateLimited = false;
            lock (rateLimitInfo)
            {
                var now = DateTime.UtcNow;
                
                // Reset if time window has passed
                if (now - rateLimitInfo.FirstRequest > TimeSpan.FromMinutes(1))
                {
                    rateLimitInfo.RequestCount = 0;
                    rateLimitInfo.FirstRequest = now;
                }

                // Check if rate limit is exceeded
                if (rateLimitInfo.RequestCount >= GetRateLimit(context))
                {
                    isRateLimited = true;
                }
                else
                {
                    rateLimitInfo.RequestCount++;
                }
            }

            if (isRateLimited)
            {
                _logger.LogWarning("Rate limit exceeded for {ClientKey} on {Path}", key, context.Request.Path);
                
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"success\":false,\"message\":\"Rate limit exceeded. Please wait a moment before trying again.\"}");
                return;
            }

            await _next(context);
        }

        private static string GetClientKey(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = context.Request.Headers.UserAgent.ToString();
            return $"{clientIp}_{userAgent.GetHashCode()}";
        }

        private static int GetRateLimit(HttpContext context)
        {
            // Different limits for different endpoints
            return context.Request.Path.StartsWithSegments("/Contact") ? 5 : 60;
        }
    }

    public class RateLimitInfo
    {
        public int RequestCount { get; set; }
        public DateTime FirstRequest { get; set; } = DateTime.UtcNow;
    }
}