using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
        
        // =======================================================================================
        // SMTP CLIENT SETUP WITH SSL CERTIFICATE HANDLING
        // =======================================================================================
        
        private async Task<SmtpClient> CreateAndConnectSmtpClientAsync()
        {
            var client = new SmtpClient();
            
            // Configure certificate validation to handle revocation check issues
            client.ServerCertificateValidationCallback = ValidateServerCertificate;
            
            // Enable certificate revocation checking
            client.CheckCertificateRevocation = true;
            
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];
                
                _logger.LogDebug("Connecting to SMTP server: {Server}:{Port}", smtpServer, smtpPort);
                
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
                
                return client;
            }
            catch (Exception)
            {
                client.Dispose();
                throw;
            }
        }
        
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // If there are no SSL policy errors, accept the certificate
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            // Handle certificate chain errors specifically
            if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateChainErrors))
            {
                foreach (var status in chain.ChainStatus)
                {
                    // Accept certificates where only revocation check failed
                    if (status.Status == X509ChainStatusFlags.RevocationStatusUnknown ||
                        status.Status == X509ChainStatusFlags.OfflineRevocation)
                    {
                        continue; // Skip these specific errors
                    }
                    
                    // Any other certificate errors should be rejected
                    if (status.Status != X509ChainStatusFlags.NoError)
                    {
                        return false;
                    }
                }
                return true;
            }

            // Reject other SSL policy errors
            return false;
        }
        
        // =======================================================================================
        // EMAIL SENDING METHODS
        // =======================================================================================
        
        public async Task<bool> SendContactEmailAsync(ContactViewModel contact)
        {
            try
            {
                // Validate configuration first
                if (!await ValidateEmailConfigurationAsync())
                {
                    _logger.LogError("Email configuration is invalid");
                    return false;
                }

                var message = new MimeMessage();
                
                // Set sender and recipient
                var fromName = _configuration["EmailSettings:FromName"] ?? "Portfolio Website";
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var toEmail = _configuration["EmailSettings:ToEmail"] ?? fromEmail;
                
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress("Arif Tan", toEmail));
                
                // Set reply-to to the contact's email for easy response
                message.ReplyTo.Add(new MailboxAddress(contact.Name, contact.Email));
                
                message.Subject = $"Portfolio Contact: {contact.Subject ?? "New Message"}";
                
                var bodyBuilder = new BodyBuilder();
                
                // Create professional HTML and text versions
                bodyBuilder.HtmlBody = CreateContactEmailHtml(contact);
                bodyBuilder.TextBody = CreateContactEmailText(contact);
                
                message.Body = bodyBuilder.ToMessageBody();
                
                // Send email with proper SSL handling
                using var client = await CreateAndConnectSmtpClientAsync();
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                
                _logger.LogInformation("Contact email sent successfully from {Email}", contact.Email);
                
                // Send auto-reply in background (don't wait for it to complete)
                _ = Task.Run(async () => 
                {
                    try
                    {
                        await SendAutoReplyEmailAsync(contact);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to send auto-reply to {Email}", contact.Email);
                    }
                });
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact email from {Email}", contact.Email);
                return false;
            }
        }
        
        public async Task<bool> SendNotificationEmailAsync(string subject, string message)
        {
            try
            {
                if (!await ValidateEmailConfigurationAsync())
                {
                    _logger.LogError("Email configuration is invalid");
                    return false;
                }

                var emailMessage = new MimeMessage();
                
                var fromName = _configuration["EmailSettings:FromName"] ?? "Portfolio System";
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var toEmail = _configuration["EmailSettings:ToEmail"] ?? fromEmail;
                
                emailMessage.From.Add(new MailboxAddress(fromName, fromEmail));
                emailMessage.To.Add(new MailboxAddress("Arif Tan", toEmail));
                emailMessage.Subject = subject;
                
                var bodyBuilder = new BodyBuilder
                {
                    TextBody = message,
                    HtmlBody = $"<p>{message.Replace("\n", "<br>")}</p>"
                };
                
                emailMessage.Body = bodyBuilder.ToMessageBody();
                
                using var client = await CreateAndConnectSmtpClientAsync();
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
                
                _logger.LogInformation("Notification email sent successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification email");
                return false;
            }
        }
        
        public async Task<bool> SendAutoReplyEmailAsync(ContactViewModel contact)
        {
            try
            {
                if (!await ValidateEmailConfigurationAsync())
                {
                    return false;
                }

                var message = new MimeMessage();
                
                var fromName = _configuration["EmailSettings:FromName"] ?? "Arif Tan";
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress(contact.Name, contact.Email));
                message.Subject = "Thank you for contacting me - Arif Tan";
                
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = CreateAutoReplyEmailHtml(contact);
                bodyBuilder.TextBody = CreateAutoReplyEmailText(contact);
                
                message.Body = bodyBuilder.ToMessageBody();
                
                using var client = await CreateAndConnectSmtpClientAsync();
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                
                _logger.LogInformation("Auto-reply email sent to {Email}", contact.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send auto-reply email to {Email}", contact.Email);
                return false;
            }
        }
        
        public async Task<bool> ValidateEmailConfigurationAsync()
        {
            try
            {
                var requiredSettings = new[]
                {
                    "EmailSettings:SmtpServer",
                    "EmailSettings:SmtpPort",
                    "EmailSettings:Username",
                    "EmailSettings:Password",
                    "EmailSettings:FromEmail"
                };

                foreach (var setting in requiredSettings)
                {
                    if (string.IsNullOrWhiteSpace(_configuration[setting]))
                    {
                        _logger.LogError("Missing email configuration: {Setting}", setting);
                        return false;
                    }
                }

                // Test SMTP connection (optional - only in debug mode)
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    try
                    {
                        using var client = await CreateAndConnectSmtpClientAsync();
                        await client.DisconnectAsync(true);
                        _logger.LogDebug("SMTP connection test successful");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "SMTP connection test failed");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating email configuration");
                return false;
            }
        }
        
        // =======================================================================================
        // HELPER METHODS FOR EMAIL TEMPLATES
        // =======================================================================================
        
        private string CreateContactEmailHtml(ContactViewModel contact)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head><meta charset='utf-8'><title>New Contact Form Submission</title></head>");
            sb.AppendLine("<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>");
            sb.AppendLine("<div style='max-width: 600px; margin: 0 auto; padding: 20px;'>");
            sb.AppendLine("<h2 style='color: #0066cc; border-bottom: 2px solid #0066cc; padding-bottom: 10px;'>New Contact Form Submission</h2>");
            sb.AppendLine("<table style='width: 100%; border-collapse: collapse;'>");
            sb.AppendLine($"<tr><td style='padding: 8px; border-bottom: 1px solid #ddd; font-weight: bold;'>Name:</td><td style='padding: 8px; border-bottom: 1px solid #ddd;'>{contact.Name}</td></tr>");
            sb.AppendLine($"<tr><td style='padding: 8px; border-bottom: 1px solid #ddd; font-weight: bold;'>Email:</td><td style='padding: 8px; border-bottom: 1px solid #ddd;'><a href='mailto:{contact.Email}'>{contact.Email}</a></td></tr>");
            
            if (!string.IsNullOrWhiteSpace(contact.Company))
                sb.AppendLine($"<tr><td style='padding: 8px; border-bottom: 1px solid #ddd; font-weight: bold;'>Company:</td><td style='padding: 8px; border-bottom: 1px solid #ddd;'>{contact.Company}</td></tr>");
            
            if (!string.IsNullOrWhiteSpace(contact.Phone))
                sb.AppendLine($"<tr><td style='padding: 8px; border-bottom: 1px solid #ddd; font-weight: bold;'>Phone:</td><td style='padding: 8px; border-bottom: 1px solid #ddd;'>{contact.Phone}</td></tr>");
            
            sb.AppendLine($"<tr><td style='padding: 8px; border-bottom: 1px solid #ddd; font-weight: bold;'>Subject:</td><td style='padding: 8px; border-bottom: 1px solid #ddd;'>{contact.Subject ?? "No subject"}</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<div style='margin-top: 20px;'>");
            sb.AppendLine("<h3 style='color: #0066cc;'>Message:</h3>");
            sb.AppendLine($"<div style='background-color: #f8f9fa; padding: 15px; border-left: 4px solid #0066cc; white-space: pre-line;'>{contact.Message}</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<hr style='margin: 30px 0; border: none; border-top: 1px solid #ddd;'>");
            sb.AppendLine($"<p style='color: #666; font-size: 12px;'><em>Sent from Arif Tan Portfolio website on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</em></p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            
            return sb.ToString();
        }
        
        private string CreateContactEmailText(ContactViewModel contact)
        {
            var sb = new StringBuilder();
            sb.AppendLine("NEW CONTACT FORM SUBMISSION");
            sb.AppendLine("=" + new string('=', 30));
            sb.AppendLine($"Name: {contact.Name}");
            sb.AppendLine($"Email: {contact.Email}");
            
            if (!string.IsNullOrWhiteSpace(contact.Company))
                sb.AppendLine($"Company: {contact.Company}");
            
            if (!string.IsNullOrWhiteSpace(contact.Phone))
                sb.AppendLine($"Phone: {contact.Phone}");
            
            sb.AppendLine($"Subject: {contact.Subject ?? "No subject"}");
            sb.AppendLine();
            sb.AppendLine("MESSAGE:");
            sb.AppendLine("-" + new string('-', 20));
            sb.AppendLine(contact.Message);
            sb.AppendLine();
            sb.AppendLine($"Sent from Arif Tan Portfolio website on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            
            return sb.ToString();
        }
        
        private string CreateAutoReplyEmailHtml(ContactViewModel contact)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head><meta charset='utf-8'><title>Thank you for contacting me</title></head>");
            sb.AppendLine("<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>");
            sb.AppendLine("<div style='max-width: 600px; margin: 0 auto; padding: 20px;'>");
            sb.AppendLine("<h2 style='color: #0066cc;'>Thank you for contacting me!</h2>");
            sb.AppendLine($"<p>Dear {contact.Name},</p>");
            sb.AppendLine("<p>Thank you for reaching out through my portfolio website. I have received your message and will get back to you within 24-48 hours.</p>");
            sb.AppendLine("<p>In the meantime, feel free to:</p>");
            sb.AppendLine("<ul>");
            sb.AppendLine("<li>Check out my <a href='https://github.com/Ariftan77'>GitHub projects</a></li>");
            sb.AppendLine("<li>Connect with me on <a href='https://www.linkedin.com/in/ariftan2212/'>LinkedIn</a></li>");
            sb.AppendLine("<li>Read my latest blog posts on software engineering</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("<p>Best regards,<br><strong>Arif Tan</strong><br>Software Engineer<br>Batam, Indonesia</p>");
            sb.AppendLine("<hr style='margin: 30px 0; border: none; border-top: 1px solid #ddd;'>");
            sb.AppendLine("<p style='color: #666; font-size: 12px;'>This is an automated response. Please do not reply to this email.</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            
            return sb.ToString();
        }
        
        private string CreateAutoReplyEmailText(ContactViewModel contact)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Dear {contact.Name},");
            sb.AppendLine();
            sb.AppendLine("Thank you for reaching out through my portfolio website. I have received your message and will get back to you within 24-48 hours.");
            sb.AppendLine();
            sb.AppendLine("In the meantime, feel free to:");
            sb.AppendLine("• Check out my GitHub projects: https://github.com/Ariftan77");
            sb.AppendLine("• Connect with me on LinkedIn: https://www.linkedin.com/in/ariftan2212/");
            sb.AppendLine("• Read my latest blog posts on software engineering");
            sb.AppendLine();
            sb.AppendLine("Best regards,");
            sb.AppendLine("Arif Tan");
            sb.AppendLine("Software Engineer");
            sb.AppendLine("Batam, Indonesia");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine("This is an automated response. Please do not reply to this email.");
            
            return sb.ToString();
        }
    }
}