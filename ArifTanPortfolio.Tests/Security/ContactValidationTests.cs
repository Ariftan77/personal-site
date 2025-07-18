using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using ArifTanPortfolio.Models.ViewModels;
using ArifTanPortfolio.Pages;
using ArifTanPortfolio.Services;
using Xunit;

namespace ArifTanPortfolio.Tests.Security
{
    public class ContactValidationTests
    {
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IPortfolioService> _mockPortfolioService;
        private readonly Mock<ILogger<ContactModel>> _mockLogger;
        private readonly ContactModel _contactModel;

        public ContactValidationTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockPortfolioService = new Mock<IPortfolioService>();
            _mockLogger = new Mock<ILogger<ContactModel>>();
            
            _contactModel = new ContactModel(
                _mockEmailService.Object,
                _mockPortfolioService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public void ContactViewModel_ShouldRequireName()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "",
                Email = "test@example.com",
                Message = "This is a test message"
            };

            // Act
            var validationResults = ValidateModel(contact);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Name"));
        }

        [Fact]
        public void ContactViewModel_ShouldRequireValidEmail()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "invalid-email",
                Message = "This is a test message"
            };

            // Act
            var validationResults = ValidateModel(contact);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Email"));
        }

        [Fact]
        public void ContactViewModel_ShouldRequireMessage()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "test@example.com",
                Message = ""
            };

            // Act
            var validationResults = ValidateModel(contact);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Message"));
        }

        [Fact]
        public void ContactViewModel_ShouldEnforceMessageLength()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "test@example.com",
                Message = "Short"
            };

            // Act
            var validationResults = ValidateModel(contact);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Message"));
        }

        [Fact]
        public void ContactViewModel_ShouldEnforceStringLengthLimits()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = new string('a', 101), // Exceeds 100 character limit
                Email = "test@example.com",
                Message = "This is a valid message length"
            };

            // Act
            var validationResults = ValidateModel(contact);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Name"));
        }

        [Theory]
        [InlineData("test@test.com")]
        [InlineData("user@example.com")]
        [InlineData("admin@tempmail.com")]
        public void ContactValidation_ShouldRejectSuspiciousEmails(string email)
        {
            // Arrange
            _contactModel.Contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = email,
                Message = "This is a test message"
            };

            // This would test the server-side validation in the ContactModel
            // In a real implementation, you'd need to expose the validation method or test it through the full action
        }

        [Theory]
        [InlineData("Check out this website http://spam.com")]
        [InlineData("Buy viagra now!")]
        [InlineData("Cryptocurrency investment opportunity")]
        public void ContactValidation_ShouldRejectSpamContent(string message)
        {
            // Arrange
            _contactModel.Contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "test@legitimate.com",
                Message = message
            };

            // This would test the server-side spam detection
            // In a real implementation, you'd need to expose the validation method or test it through the full action
        }

        [Fact]
        public void ContactValidation_ShouldAcceptValidContact()
        {
            // Arrange
            var contact = new ContactViewModel
            {
                Name = "John Doe",
                Email = "john@legitimate.com",
                Message = "This is a legitimate inquiry about your services.",
                Company = "ABC Corp",
                Phone = "+1-555-123-4567",
                Subject = "Job Opportunity"
            };

            // Act
            var validationResults = ValidateModel(contact);

            // Assert
            Assert.Empty(validationResults);
        }

        private static List<System.ComponentModel.DataAnnotations.ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}