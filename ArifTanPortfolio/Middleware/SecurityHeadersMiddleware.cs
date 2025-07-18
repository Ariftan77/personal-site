namespace ArifTanPortfolio.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityHeadersMiddleware> _logger;

        public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add security headers
            var headers = context.Response.Headers;

            // Prevent clickjacking
            headers["X-Frame-Options"] = "DENY";

            // Prevent MIME type sniffing
            headers["X-Content-Type-Options"] = "nosniff";

            // Enable XSS protection
            headers["X-XSS-Protection"] = "1; mode=block";

            // Referrer policy
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Content Security Policy
            headers["Content-Security-Policy"] = 
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com https://cdn.jsdelivr.net https://www.googletagmanager.com https://www.google-analytics.com; " +
                "style-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com https://cdn.jsdelivr.net https://fonts.googleapis.com; " +
                "font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com; " +
                "img-src 'self' data: https: http:; " +
                "connect-src 'self' https://www.google-analytics.com; " +
                "form-action 'self'; " +
                "base-uri 'self'; " +
                "object-src 'none'; " +
                "frame-ancestors 'none'";

            // Permissions Policy (formerly Feature Policy)
            headers["Permissions-Policy"] = 
                "camera=(), microphone=(), geolocation=(), payment=(), usb=(), magnetometer=(), gyroscope=(), accelerometer=()";

            // Strict Transport Security (HSTS) - only for HTTPS
            if (context.Request.IsHttps)
            {
                headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
            }

            // Remove server header
            headers.Remove("Server");

            await _next(context);
        }
    }
}