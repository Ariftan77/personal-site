using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ArifTanPortfolio.Models.ViewModels;

namespace ArifTanPortfolio.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        public async Task<bool> SendContactEmailAsync(ContactViewModel contact)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    _configuration["EmailSettings:FromName"], 
                    _configuration["EmailSettings:FromEmail"]));
                message.To.Add(new MailboxAddress("Arif Tan", _configuration["EmailSettings:FromEmail"]));
                message.Subject = $"Portfolio Contact: {contact.Subject ?? "New Message"}";
                
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <h2>New Contact Form Submission</h2>
                        <p><strong>Name:</strong> {contact.Name}</p>
                        <p><strong>Email:</strong> {contact.Email}</p>
                        <p><strong>Company:</strong> {contact.Company ?? "Not provided"}</p>
                        <p><strong>Phone:</strong> {contact.Phone ?? "Not provided"}</p>
                        <p><strong>Subject:</strong> {contact.Subject ?? "No subject"}</p>
                        <p><strong>Message:</strong></p>
                        <div style='background-color: #f5f5f5; padding: 15px; border-left: 4px solid #007bff;'>
                            {contact.Message.Replace("\n", "<br>")}
                        </div>
                        <hr>
                        <p><em>Sent from Arif Tan Portfolio website</em></p>"
                };
                
                message.Body = bodyBuilder.ToMessageBody();
                
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _configuration["EmailSettings:SmtpServer"], 
                    int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587"), 
                    SecureSocketOptions.StartTls);
                    
                await client.AuthenticateAsync(
                    _configuration["EmailSettings:Username"], 
                    _configuration["EmailSettings:Password"]);
                    
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact email");
                return false;
            }
        }
        
        public async Task<bool> SendNotificationEmailAsync(string subject, string message)
        {
            // Implementation for sending notification emails
            return true;
        }
    }
}