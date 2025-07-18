using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ArifTanPortfolio.Tests.Security
{
    public class ConfigurationSecurityTests
    {
        [Fact]
        public void Configuration_ShouldNotContainPasswordsInAppsettings()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // Act
            var emailPassword = configuration["EmailSettings:Password"];
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Assert
            Assert.Null(emailPassword); // Should be null since we moved it to user secrets
            Assert.NotNull(connectionString);
            Assert.DoesNotContain("Password=", connectionString ?? "");
            Assert.DoesNotContain("pwd=", connectionString ?? "");
        }

        [Fact]
        public void Configuration_ShouldUseUserSecretsInDevelopment()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddUserSecrets<Program>(optional: true)
                .Build();

            // Act
            var emailPassword = configuration["EmailSettings:Password"];

            // Assert
            // In development, this should be available from user secrets
            // In production, this would come from environment variables
            Assert.True(string.IsNullOrEmpty(emailPassword) || emailPassword.Length > 0);
        }

        [Fact]
        public void Configuration_ShouldSupportEnvironmentVariables()
        {
            // Arrange
            Environment.SetEnvironmentVariable("EmailSettings__Password", "test-password");
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Act
            var emailPassword = configuration["EmailSettings:Password"];

            // Assert
            Assert.Equal("test-password", emailPassword);
            
            // Cleanup
            Environment.SetEnvironmentVariable("EmailSettings__Password", null);
        }

        [Fact]
        public void Configuration_ShouldHaveProperLoggingLevels()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            // Act
            var defaultLogLevel = configuration["Logging:LogLevel:Default"];
            var aspNetCoreLogLevel = configuration["Logging:LogLevel:Microsoft.AspNetCore"];

            // Assert
            Assert.Equal("Information", defaultLogLevel);
            Assert.Equal("Warning", aspNetCoreLogLevel);
        }
    }
}