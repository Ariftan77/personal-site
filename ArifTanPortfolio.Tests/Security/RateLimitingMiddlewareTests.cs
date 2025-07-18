using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ArifTanPortfolio.Middleware;
using Xunit;
using System.Net;

namespace ArifTanPortfolio.Tests.Security
{
    public class RateLimitingMiddlewareTests
    {
        private readonly Mock<ILogger<RateLimitingMiddleware>> _mockLogger;
        private readonly RateLimitingMiddleware _middleware;

        public RateLimitingMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<RateLimitingMiddleware>>();
            _middleware = new RateLimitingMiddleware(
                context => Task.CompletedTask,
                _mockLogger.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldAllowRequestsWithinLimit()
        {
            // Arrange
            var context = CreateHttpContext("/test");

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(200, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldApplyContactFormRateLimit()
        {
            // Arrange
            var uniqueUserAgent = "Rate Limit Test Agent " + Guid.NewGuid();
            var context = CreateHttpContext("/Contact");
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.200");
            context.Request.Headers["User-Agent"] = uniqueUserAgent;
            
            // Act - Make 6 requests (limit is 5 for contact)
            for (int i = 0; i < 6; i++)
            {
                context.Response.Clear();
                await _middleware.InvokeAsync(context);
            }

            // Assert
            Assert.Equal((int)HttpStatusCode.TooManyRequests, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldAllowHigherLimitForNonContactEndpoints()
        {
            // Arrange
            var uniqueUserAgent = "Non-Contact Test Agent " + Guid.NewGuid();
            var context = CreateHttpContext("/blog");
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.50");
            context.Request.Headers["User-Agent"] = uniqueUserAgent;
            
            // Act - Make 10 requests (should be allowed for non-contact endpoints)
            for (int i = 0; i < 10; i++)
            {
                context.Response.Clear();
                await _middleware.InvokeAsync(context);
            }

            // Assert
            Assert.Equal(200, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldAllowFirstRequestForUniqueClient()
        {
            // Arrange
            var context = CreateHttpContext("/Contact");
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.100");
            context.Request.Headers["User-Agent"] = "Unique Test Agent " + Guid.NewGuid();
            
            // Act
            await _middleware.InvokeAsync(context);
            
            // Assert
            Assert.Equal(200, context.Response.StatusCode);
        }

        private static DefaultHttpContext CreateHttpContext(string path)
        {
            var context = new DefaultHttpContext();
            context.Request.Path = path;
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            context.Request.Headers["User-Agent"] = "Test Agent";
            return context;
        }
    }
}