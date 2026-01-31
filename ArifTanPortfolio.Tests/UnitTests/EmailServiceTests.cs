using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ArifTanPortfolio.Services;
using ArifTanPortfolio.Models.ViewModels;

namespace ArifTanPortfolio.Tests.UnitTests
{
    public class EmailServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly Mock<IConfigurationSection> _mockEmailSection;

        public EmailServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<EmailService>>();
            _mockEmailSection = new Mock<IConfigurationSection>();
        }

        #region Configuration Validation Tests

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithAllRequiredSettings_ShouldReturnTrue()
        {
            // Arrange
            SetupValidConfiguration();
            var service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithMissingSmtpServer_ShouldReturnFalse()
        {
            // Arrange
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpServer"]).Returns((string)null!);
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpPort"]).Returns("587");
            _mockConfiguration.Setup(x => x["EmailSettings:Username"]).Returns("user");
            _mockConfiguration.Setup(x => x["EmailSettings:Password"]).Returns("pass");
            _mockConfiguration.Setup(x => x["EmailSettings:FromEmail"]).Returns("from@example.com");

            var service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithMissingSmtpPort_ShouldReturnFalse()
        {
            // Arrange
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpServer"]).Returns("smtp.example.com");
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpPort"]).Returns((string)null!);
            _mockConfiguration.Setup(x => x["EmailSettings:Username"]).Returns("user");
            _mockConfiguration.Setup(x => x["EmailSettings:Password"]).Returns("pass");
            _mockConfiguration.Setup(x => x["EmailSettings:FromEmail"]).Returns("from@example.com");

            var service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithMissingUsername_ShouldReturnFalse()
        {
            // Arrange
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpServer"]).Returns("smtp.example.com");
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpPort"]).Returns("587");
            _mockConfiguration.Setup(x => x["EmailSettings:Username"]).Returns((string)null!);
            _mockConfiguration.Setup(x => x["EmailSettings:Password"]).Returns("pass");
            _mockConfiguration.Setup(x => x["EmailSettings:FromEmail"]).Returns("from@example.com");

            var service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithMissingPassword_ShouldReturnFalse()
        {
            // Arrange
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpServer"]).Returns("smtp.example.com");
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpPort"]).Returns("587");
            _mockConfiguration.Setup(x => x["EmailSettings:Username"]).Returns("user");
            _mockConfiguration.Setup(x => x["EmailSettings:Password"]).Returns((string)null!);
            _mockConfiguration.Setup(x => x["EmailSettings:FromEmail"]).Returns("from@example.com");

            var service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithMissingFromEmail_ShouldReturnFalse()
        {
            // Arrange
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpServer"]).Returns("smtp.example.com");
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpPort"]).Returns("587");
            _mockConfiguration.Setup(x => x["EmailSettings:Username"]).Returns("user");
            _mockConfiguration.Setup(x => x["EmailSettings:Password"]).Returns("pass");
            _mockConfiguration.Setup(x => x["EmailSettings:FromEmail"]).Returns((string)null!);

            var service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithEmptyStrings_ShouldReturnFalse()
        {
            // Arrange
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpServer"]).Returns("   ");
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpPort"]).Returns("587");
            _mockConfiguration.Setup(x => x["EmailSettings:Username"]).Returns("user");
            _mockConfiguration.Setup(x => x["EmailSettings:Password"]).Returns("pass");
            _mockConfiguration.Setup(x => x["EmailSettings:FromEmail"]).Returns("from@example.com");

            var service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region ContactViewModel Tests

        [Fact]
        public void ContactViewModel_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                Subject = "Testing",
                Message = "This is a test message with enough characters",
                Company = "Test Company",
                Phone = "+1234567890"
            };

            // Assert
            contact.Name.Should().Be("John Doe");
            contact.Email.Should().Be("john@example.com");
            contact.Subject.Should().Be("Testing");
            contact.Message.Should().Be("This is a test message with enough characters");
            contact.Company.Should().Be("Test Company");
            contact.Phone.Should().Be("+1234567890");
        }

        [Fact]
        public void ContactViewModel_WithMinimalData_ShouldCreateSuccessfully()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                Message = "This is a test message"
            };

            // Assert
            contact.Name.Should().Be("John Doe");
            contact.Email.Should().Be("john@example.com");
            contact.Message.Should().Be("This is a test message");
            contact.Subject.Should().BeNull();
            contact.Company.Should().BeNull();
            contact.Phone.Should().BeNull();
        }

        [Fact]
        public void ContactViewModel_WithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "O'Brien & Smith",
                Email = "test+tag@example.com",
                Subject = "Question about <project>",
                Message = "Message with special chars: @#$%^&*()",
                Company = "Company & Co."
            };

            // Assert
            contact.Name.Should().Contain("'");
            contact.Name.Should().Contain("&");
            contact.Email.Should().Contain("+");
            contact.Subject.Should().Contain("<");
            contact.Subject.Should().Contain(">");
            contact.Message.Should().Contain("@#$%^&*()");
        }

        [Fact]
        public void ContactViewModel_WithLongMessage_ShouldStore()
        {
            // Arrange
            var longMessage = new string('a', 1999); // Just under 2000 limit

            // Act
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                Message = longMessage
            };

            // Assert
            contact.Message.Length.Should().Be(1999);
        }

        [Fact]
        public void ContactViewModel_WithMultipleEmails_ShouldStoreAsProvided()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john.doe+test@example.co.uk",
                Message = "Test message for email validation"
            };

            // Assert
            contact.Email.Should().Be("john.doe+test@example.co.uk");
        }

        #endregion

        #region Email Template Content Tests

        [Theory]
        [InlineData("John Doe", "john@example.com", "Test Subject", "Test message")]
        [InlineData("Jane Smith", "jane@test.com", null, "Another test message")]
        [InlineData("Bob O'Brien", "bob@company.com", "Inquiry", "Message with special chars: <>&")]
        public void ContactEmail_ShouldContainRequiredInformation(string name, string email, string? subject, string message)
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message
            };

            // Act - We can't directly test private methods, but we can verify the model is set up correctly
            // In a real scenario, you'd test the actual email content after sending

            // Assert
            contact.Name.Should().Be(name);
            contact.Email.Should().Be(email);
            contact.Subject.Should().Be(subject);
            contact.Message.Should().Be(message);
        }

        [Fact]
        public void ContactViewModel_WithCompanyAndPhone_ShouldStoreOptionalFields()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                Message = "Test message",
                Company = "Acme Corp",
                Phone = "+1-555-123-4567"
            };

            // Assert
            contact.Company.Should().Be("Acme Corp");
            contact.Phone.Should().Be("+1-555-123-4567");
        }

        [Fact]
        public void ContactViewModel_WithEmptyOptionalFields_ShouldBeNull()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                Message = "Test message"
            };

            // Assert
            contact.Company.Should().BeNull();
            contact.Phone.Should().BeNull();
            contact.Subject.Should().BeNull();
        }

        #endregion

        #region Input Sanitization Tests

        [Fact]
        public void ContactViewModel_WithLeadingTrailingSpaces_ShouldStoreAsProvided()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "  John Doe  ",
                Email = "  john@example.com  ",
                Message = "  Test message  "
            };

            // Assert - Note: Trimming happens in the service layer, not the model
            contact.Name.Should().Be("  John Doe  ");
            contact.Email.Should().Be("  john@example.com  ");
            contact.Message.Should().Be("  Test message  ");
        }

        [Fact]
        public void ContactViewModel_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "José García 李明",
                Email = "jose@example.com",
                Message = "Message with unicode: é, ñ, ü, 中文, 日本語"
            };

            // Assert
            contact.Name.Should().Contain("José");
            contact.Name.Should().Contain("García");
            contact.Name.Should().Contain("李明");
            contact.Message.Should().Contain("中文");
            contact.Message.Should().Contain("日本語");
        }

        #endregion

        #region Email Format Tests

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@example.com")]
        [InlineData("user+tag@example.co.uk")]
        [InlineData("user_name@subdomain.example.com")]
        public void ContactViewModel_WithValidEmailFormats_ShouldAccept(string email)
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = email,
                Message = "Test message"
            };

            // Assert
            contact.Email.Should().Be(email);
        }

        #endregion

        #region Helper Methods

        private void SetupValidConfiguration()
        {
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpServer"]).Returns("smtp.example.com");
            _mockConfiguration.Setup(x => x["EmailSettings:SmtpPort"]).Returns("587");
            _mockConfiguration.Setup(x => x["EmailSettings:Username"]).Returns("user@example.com");
            _mockConfiguration.Setup(x => x["EmailSettings:Password"]).Returns("password123");
            _mockConfiguration.Setup(x => x["EmailSettings:FromEmail"]).Returns("noreply@example.com");
            _mockConfiguration.Setup(x => x["EmailSettings:FromName"]).Returns("Portfolio");
            _mockConfiguration.Setup(x => x["EmailSettings:ToEmail"]).Returns("admin@example.com");
        }

        #endregion
    }
}
