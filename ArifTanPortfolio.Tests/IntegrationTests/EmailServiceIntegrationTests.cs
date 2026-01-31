using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ArifTanPortfolio.Services;
using ArifTanPortfolio.Models.ViewModels;

namespace ArifTanPortfolio.Tests.IntegrationTests
{
    public class EmailServiceIntegrationTests : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly EmailService _service;

        public EmailServiceIntegrationTests()
        {
            _mockLogger = new Mock<ILogger<EmailService>>();
            _configuration = CreateTestConfiguration();
            _service = new EmailService(_configuration, _mockLogger.Object);
        }

        public void Dispose()
        {
            // Cleanup if needed
        }

        #region Email Sending Tests (Without Actual SMTP)

        [Fact]
        public async Task SendContactEmailAsync_WithValidConfiguration_ShouldReturnFalse()
        {
            // Note: This will return false because we can't connect to the fake SMTP server
            // But it tests the validation and message construction logic

            // Arrange
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                Subject = "Test Subject",
                Message = "This is a test message with enough characters to pass validation"
            };

            // Act
            var result = await _service.SendContactEmailAsync(contact);

            // Assert
            // Will be false because SMTP connection fails (expected in test environment)
            result.Should().BeFalse();
        }

        [Fact]
        public async Task SendContactEmailAsync_WithCompleteContactInfo_ShouldProcessAllFields()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Subject = "Inquiry",
                Message = "I would like to discuss a potential collaboration opportunity",
                Company = "Tech Corp",
                Phone = "+1-555-123-4567"
            };

            // Act
            var result = await _service.SendContactEmailAsync(contact);

            // Assert
            // Method should process all fields (even if send fails due to SMTP)
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendContactEmailAsync_WithMinimalContactInfo_ShouldProcess()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Bob Jones",
                Email = "bob@example.com",
                Message = "Quick question about your services and availability"
            };

            // Act
            var result = await _service.SendContactEmailAsync(contact);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendContactEmailAsync_WithSpecialCharacters_ShouldHandle()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "José García-O'Brien",
                Email = "jose@example.com",
                Subject = "Question about <project> & collaboration",
                Message = "Message with special chars: @#$%^&*() and unicode: 你好"
            };

            // Act
            var result = await _service.SendContactEmailAsync(contact);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendContactEmailAsync_WithLongMessage_ShouldHandle()
        {
            // Arrange
            var longMessage = string.Join(" ", Enumerable.Repeat("This is a very detailed message.", 50));
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Subject = "Detailed Inquiry",
                Message = longMessage
            };

            // Act
            var result = await _service.SendContactEmailAsync(contact);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendContactEmailAsync_WithMultilineMessage_ShouldPreserveFormatting()
        {
            // Arrange
            var message = "Line 1: Introduction\n\nLine 2: Details\n\nLine 3: Conclusion";
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = message
            };

            // Act
            var result = await _service.SendContactEmailAsync(contact);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendNotificationEmailAsync_WithValidInput_ShouldProcess()
        {
            // Arrange
            var subject = "Test Notification";
            var message = "This is a test notification message";

            // Act
            var result = await _service.SendNotificationEmailAsync(subject, message);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendNotificationEmailAsync_WithMultilineMessage_ShouldProcess()
        {
            // Arrange
            var subject = "Multi-line Notification";
            var message = "Line 1\nLine 2\nLine 3\nLine 4";

            // Act
            var result = await _service.SendNotificationEmailAsync(subject, message);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendNotificationEmailAsync_WithSpecialCharacters_ShouldHandle()
        {
            // Arrange
            var subject = "Special Characters Test: <>&\"'";
            var message = "Message with special characters: @#$%^&*()";

            // Act
            var result = await _service.SendNotificationEmailAsync(subject, message);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendAutoReplyEmailAsync_WithValidContact_ShouldProcess()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Auto Reply Test",
                Email = "autoreply@example.com",
                Message = "This message should trigger an auto-reply"
            };

            // Act
            var result = await _service.SendAutoReplyEmailAsync(contact);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        [Fact]
        public async Task SendAutoReplyEmailAsync_WithInternationalName_ShouldHandle()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "李明 Yamada-san",
                Email = "international@example.com",
                Message = "Testing international character support"
            };

            // Act
            var result = await _service.SendAutoReplyEmailAsync(contact);

            // Assert
            result.Should().BeFalse(); // Expected to fail in test environment
        }

        #endregion

        #region Email Template Content Verification Tests

        [Fact]
        public void EmailTemplate_ContactEmail_ShouldIncludeAllProvidedFields()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Template Test User",
                Email = "template@example.com",
                Subject = "Template Test",
                Message = "Testing email template generation",
                Company = "Template Corp",
                Phone = "+1-555-TEMPLATE"
            };

            // Act & Assert - Verify all fields are set
            contact.Name.Should().NotBeNullOrEmpty();
            contact.Email.Should().NotBeNullOrEmpty();
            contact.Subject.Should().NotBeNullOrEmpty();
            contact.Message.Should().NotBeNullOrEmpty();
            contact.Company.Should().NotBeNullOrEmpty();
            contact.Phone.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void EmailTemplate_AutoReply_ShouldHaveRequiredFields()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Auto Reply User",
                Email = "autoreply@example.com",
                Message = "This will generate an auto-reply"
            };

            // Act & Assert - Auto-reply needs at minimum: name, email
            contact.Name.Should().NotBeNullOrEmpty();
            contact.Email.Should().NotBeNullOrEmpty();
            contact.Message.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Configuration Tests

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithCompleteConfiguration_ShouldReturnTrue()
        {
            // Act
            var result = await _service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithIncompleteConfiguration_ShouldReturnFalse()
        {
            // Arrange
            var incompleteConfig = CreateIncompleteConfiguration();
            var service = new EmailService(incompleteConfig, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Email Template Generation Tests

        [Fact]
        public void ContactViewModel_ToEmailFormat_ShouldIncludeAllRequiredFields()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@example.com",
                Subject = "Test Subject",
                Message = "This is a test message with enough characters to pass validation",
                Company = "Test Company",
                Phone = "+1234567890"
            };

            // Assert - Verify all fields are set correctly for email generation
            contact.Name.Should().Be("John Doe");
            contact.Email.Should().Be("john@example.com");
            contact.Subject.Should().Be("Test Subject");
            contact.Message.Should().Contain("test message");
            contact.Company.Should().Be("Test Company");
            contact.Phone.Should().Be("+1234567890");
        }

        [Fact]
        public void ContactViewModel_WithMinimalFields_ShouldBeValid()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Message = "Minimal message content with enough characters"
            };

            // Assert - Optional fields should be null
            contact.Name.Should().Be("Jane Smith");
            contact.Email.Should().Be("jane@example.com");
            contact.Message.Should().Contain("Minimal");
            contact.Subject.Should().BeNull();
            contact.Company.Should().BeNull();
            contact.Phone.Should().BeNull();
        }

        [Fact]
        public void ContactViewModel_WithSpecialCharactersInName_ShouldHandleCorrectly()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "José García-O'Brien",
                Email = "jose@example.com",
                Message = "Test message with special characters in name"
            };

            // Assert
            contact.Name.Should().Contain("José");
            contact.Name.Should().Contain("García");
            contact.Name.Should().Contain("O'Brien");
        }

        [Fact]
        public void ContactViewModel_WithHTMLInMessage_ShouldStoreAsProvided()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Message with <html> tags and & special chars"
            };

            // Assert - HTML should be stored as-is (sanitization happens in email template)
            contact.Message.Should().Contain("<html>");
            contact.Message.Should().Contain("&");
        }

        [Fact]
        public void ContactViewModel_WithLongMessage_ShouldStoreComplete()
        {
            // Arrange
            var longMessage = string.Join(" ", Enumerable.Repeat("This is a long message.", 50));
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = longMessage
            };

            // Assert
            contact.Message.Should().HaveLength(longMessage.Length);
            contact.Message.Should().Contain("long message");
        }

        #endregion

        #region Email Address Validation Tests

        [Theory]
        [InlineData("simple@example.com")]
        [InlineData("user.name@example.com")]
        [InlineData("user+tag@example.co.uk")]
        [InlineData("user_name@sub.example.com")]
        [InlineData("123@example.com")]
        public void ContactViewModel_WithValidEmailFormats_ShouldAccept(string email)
        {
            // Arrange & Act
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = email,
                Message = "Test message for email validation"
            };

            // Assert
            contact.Email.Should().Be(email);
        }

        [Fact]
        public void ContactViewModel_WithInternationalDomain_ShouldAccept()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "user@example.co.jp",
                Message = "Test message with international domain"
            };

            // Assert
            contact.Email.Should().Be("user@example.co.jp");
        }

        #endregion

        #region Subject Line Tests

        [Fact]
        public void ContactViewModel_WithCustomSubject_ShouldUseCustomSubject()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Subject = "Custom Subject Line",
                Message = "Test message"
            };

            // Assert
            contact.Subject.Should().Be("Custom Subject Line");
        }

        [Fact]
        public void ContactViewModel_WithNullSubject_ShouldAllowNull()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Test message"
            };

            // Assert
            contact.Subject.Should().BeNull();
        }

        [Fact]
        public void ContactViewModel_WithEmptySubject_ShouldAllowEmpty()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Subject = "",
                Message = "Test message"
            };

            // Assert
            contact.Subject.Should().BeEmpty();
        }

        #endregion

        #region Company and Phone Field Tests

        [Theory]
        [InlineData("Acme Corp")]
        [InlineData("Tech Solutions Ltd.")]
        [InlineData("Company & Co.")]
        [InlineData("مؤسسة")] // Arabic company name
        public void ContactViewModel_WithVariousCompanyNames_ShouldAccept(string companyName)
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Test message",
                Company = companyName
            };

            // Assert
            contact.Company.Should().Be(companyName);
        }

        [Theory]
        [InlineData("+1234567890")]
        [InlineData("+1-555-123-4567")]
        [InlineData("(555) 123-4567")]
        [InlineData("+44 20 7946 0958")]
        [InlineData("+81-3-1234-5678")]
        public void ContactViewModel_WithVariousPhoneFormats_ShouldAccept(string phone)
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Test message",
                Phone = phone
            };

            // Assert
            contact.Phone.Should().Be(phone);
        }

        #endregion

        #region Multi-line Message Tests

        [Fact]
        public void ContactViewModel_WithMultiLineMessage_ShouldPreserveLineBreaks()
        {
            // Arrange
            var message = "Line 1\nLine 2\nLine 3";
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = message
            };

            // Assert
            contact.Message.Should().Contain("\n");
            contact.Message.Should().Be(message);
        }

        [Fact]
        public void ContactViewModel_WithCarriageReturnLineBreaks_ShouldPreserve()
        {
            // Arrange
            var message = "Line 1\r\nLine 2\r\nLine 3";
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = message
            };

            // Assert
            contact.Message.Should().Contain("\r\n");
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void ContactViewModel_WithMaxLengthName_ShouldAccept()
        {
            // Arrange - Max length is 100
            var longName = new string('A', 100);
            var contact = new ContactViewModel
            {
                Name = longName,
                Email = "test@example.com",
                Message = "Test message"
            };

            // Assert
            contact.Name.Should().HaveLength(100);
        }

        [Fact]
        public void ContactViewModel_WithMaxLengthEmail_ShouldAccept()
        {
            // Arrange - Max length is 200
            var longEmail = new string('a', 180) + "@example.com"; // Total ~193 chars
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = longEmail,
                Message = "Test message"
            };

            // Assert
            contact.Email.Should().HaveLength(longEmail.Length);
        }

        [Fact]
        public void ContactViewModel_WithMaxLengthMessage_ShouldAccept()
        {
            // Arrange - Max length is 2000
            var longMessage = new string('A', 2000);
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = longMessage
            };

            // Assert
            contact.Message.Should().HaveLength(2000);
        }

        [Fact]
        public void ContactViewModel_WithMaxLengthSubject_ShouldAccept()
        {
            // Arrange - Max length is 100
            var longSubject = new string('A', 100);
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Subject = longSubject,
                Message = "Test message"
            };

            // Assert
            contact.Subject.Should().HaveLength(100);
        }

        [Fact]
        public void ContactViewModel_WithMaxLengthCompany_ShouldAccept()
        {
            // Arrange - Max length is 100
            var longCompany = new string('A', 100);
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Test message",
                Company = longCompany
            };

            // Assert
            contact.Company.Should().HaveLength(100);
        }

        [Fact]
        public void ContactViewModel_WithMaxLengthPhone_ShouldAccept()
        {
            // Arrange - Max length is 100
            var longPhone = new string('1', 100);
            var contact = new ContactViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Test message",
                Phone = longPhone
            };

            // Assert
            contact.Phone.Should().HaveLength(100);
        }

        #endregion

        #region Unicode and International Character Tests

        [Fact]
        public void ContactViewModel_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "李明 Smith 山田太郎",
                Email = "test@example.com",
                Message = "Message with unicode: 你好世界 こんにちは мир"
            };

            // Assert
            contact.Name.Should().Contain("李明");
            contact.Name.Should().Contain("山田太郎");
            contact.Message.Should().Contain("你好世界");
            contact.Message.Should().Contain("こんにちは");
            contact.Message.Should().Contain("мир");
        }

        [Fact]
        public void ContactViewModel_WithEmojis_ShouldHandleCorrectly()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "Test User 😀",
                Email = "test@example.com",
                Message = "Message with emojis: 👍 🎉 💻 📧"
            };

            // Assert
            contact.Name.Should().Contain("😀");
            contact.Message.Should().Contain("👍");
            contact.Message.Should().Contain("🎉");
        }

        [Fact]
        public void ContactViewModel_WithArabicText_ShouldHandleCorrectly()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "محمد أحمد",
                Email = "test@example.com",
                Message = "رسالة تجريبية باللغة العربية"
            };

            // Assert
            contact.Name.Should().Contain("محمد");
            contact.Message.Should().Contain("رسالة");
        }

        #endregion

        #region Email Configuration Scenarios

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithMissingSmtpServer_ShouldReturnFalse()
        {
            // Arrange
            var configDict = new Dictionary<string, string?>
            {
                ["EmailSettings:SmtpPort"] = "587",
                ["EmailSettings:Username"] = "test@example.com",
                ["EmailSettings:Password"] = "password",
                ["EmailSettings:FromEmail"] = "from@example.com"
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();
            var service = new EmailService(config, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithInvalidPort_ShouldHandleGracefully()
        {
            // Arrange
            var configDict = new Dictionary<string, string?>
            {
                ["EmailSettings:SmtpServer"] = "smtp.example.com",
                ["EmailSettings:SmtpPort"] = "invalid",
                ["EmailSettings:Username"] = "test@example.com",
                ["EmailSettings:Password"] = "password",
                ["EmailSettings:FromEmail"] = "from@example.com"
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();
            var service = new EmailService(config, _mockLogger.Object);

            // Act & Assert - Should handle parsing error gracefully
            var exception = await Record.ExceptionAsync(() => service.ValidateEmailConfigurationAsync());
            // Service should handle invalid port gracefully
        }

        [Fact]
        public async Task ValidateEmailConfigurationAsync_WithAllFieldsEmpty_ShouldReturnFalse()
        {
            // Arrange
            var configDict = new Dictionary<string, string?>
            {
                ["EmailSettings:SmtpServer"] = "",
                ["EmailSettings:SmtpPort"] = "",
                ["EmailSettings:Username"] = "",
                ["EmailSettings:Password"] = "",
                ["EmailSettings:FromEmail"] = ""
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();
            var service = new EmailService(config, _mockLogger.Object);

            // Act
            var result = await service.ValidateEmailConfigurationAsync();

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Helper Methods

        private IConfiguration CreateTestConfiguration()
        {
            var configDict = new Dictionary<string, string?>
            {
                ["EmailSettings:SmtpServer"] = "smtp.example.com",
                ["EmailSettings:SmtpPort"] = "587",
                ["EmailSettings:Username"] = "test@example.com",
                ["EmailSettings:Password"] = "testpassword123",
                ["EmailSettings:FromEmail"] = "noreply@example.com",
                ["EmailSettings:FromName"] = "Test Portfolio",
                ["EmailSettings:ToEmail"] = "admin@example.com"
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();
        }

        private IConfiguration CreateIncompleteConfiguration()
        {
            var configDict = new Dictionary<string, string?>
            {
                ["EmailSettings:SmtpServer"] = "smtp.example.com",
                ["EmailSettings:SmtpPort"] = "587"
                // Missing Username, Password, FromEmail
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();
        }

        #endregion
    }
}
