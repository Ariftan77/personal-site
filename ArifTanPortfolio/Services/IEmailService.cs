using ArifTanPortfolio.Models.ViewModels;

namespace ArifTanPortfolio.Services
{
    public interface IEmailService
    {
        Task<bool> SendContactEmailAsync(ContactViewModel contact);
        Task<bool> SendNotificationEmailAsync(string subject, string message);
        // Add these NEW methods for enhanced functionality
        Task<bool> SendAutoReplyEmailAsync(ContactViewModel contact);
        Task<bool> ValidateEmailConfigurationAsync();
    }
}