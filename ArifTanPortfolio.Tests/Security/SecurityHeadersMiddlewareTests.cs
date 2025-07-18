using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ArifTanPortfolio.Middleware;
using Xunit;

namespace ArifTanPortfolio.Tests.Security
{
    public class SecurityHeadersMiddlewareTests
    {
        private readonly Mock<ILogger<SecurityHeadersMiddleware>> _mockLogger;
        private readonly SecurityHeadersMiddleware _middleware;

        public SecurityHeadersMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<SecurityHeadersMiddleware>>();
            _middleware = new SecurityHeadersMiddleware(
                context => Task.CompletedTask,
                _mockLogger.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldAddSecurityHeaders()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.Equal("DENY", context.Response.Headers["X-Frame-Options"]);
            Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
            Assert.Equal("1; mode=block", context.Response.Headers["X-XSS-Protection"]);
            Assert.Equal("strict-origin-when-cross-origin", context.Response.Headers["Referrer-Policy"]);
            Assert.True(context.Response.Headers.ContainsKey("Content-Security-Policy"));
            Assert.True(context.Response.Headers.ContainsKey("Permissions-Policy"));
        }

        [Fact]
        public async Task InvokeAsync_ShouldAddHSTSForHttpsRequests()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.Equal("max-age=31536000; includeSubDomains; preload", 
                context.Response.Headers["Strict-Transport-Security"]);
        }

        [Fact]
        public async Task InvokeAsync_ShouldNotAddHSTSForHttpRequests()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Scheme = "http";

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.False(context.Response.Headers.ContainsKey("Strict-Transport-Security"));
        }

        [Fact]
        public async Task InvokeAsync_ShouldRemoveServerHeader()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Headers["Server"] = "Test Server";

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.False(context.Response.Headers.ContainsKey("Server"));
        }

        [Fact]
        public async Task InvokeAsync_ShouldHaveProperCSPPolicy()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            var csp = context.Response.Headers["Content-Security-Policy"].ToString();
            Assert.Contains("default-src 'self'", csp);
            Assert.Contains("object-src 'none'", csp);
            Assert.Contains("frame-ancestors 'none'", csp);
        }
    }
}