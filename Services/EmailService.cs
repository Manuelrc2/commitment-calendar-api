using commitment_calendar_api.Dtos.Settings;
using commitment_calendar_api.Entities;
using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Persistence;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace commitment_calendar_api.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailSettings _emailSettings;

        public EmailService(ApplicationDbContext applicationDbContext, IEmailSettings emailSettings)
        {
            _applicationDbContext = applicationDbContext;
            _emailSettings = emailSettings;
        }

        public async Task ProcessEmails(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;
            var targetTime = DateTime.Now.ToLocalTime().AddMinutes(15);
            var appointments = await _applicationDbContext.Appointments
                .Include(appointment => appointment.User)
                .Where(appointment => appointment.StartsAt == targetTime && !appointment.IsDeleted && !appointment.NotificationSent)
                .ToListAsync(cancellationToken);
            var tasks = new List<Task>();
            foreach (var appointment in appointments)
            {
                var htmlBody = CreateHtmlEmailBody(appointment);
                tasks.Add(SendEmailAsync(appointment.User.Email, appointment.User.Name, "Upcoming appointment", htmlBody, "This is a friendly reminder that you have an upcoming appointment", cancellationToken));
            }
            await Task.WhenAll(tasks);
            //_logger.LogInformation("Emails batch sent");
        }

        private async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody, string textBody, CancellationToken cancellationToken)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;

                if (!string.IsNullOrEmpty(textBody))
                {
                    bodyBuilder.TextBody = textBody;
                }

                message.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls, cancellationToken);

                    await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);

                    await smtpClient.SendAsync(message, cancellationToken);

                    await smtpClient.DisconnectAsync(true, cancellationToken);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public string CreateHtmlEmailBody(Appointment appointment)
        {
            return $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 20px; }}
                    .header {{ color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px; }}
                    .details {{ background-color: #f8f9fa; padding: 15px; margin: 15px 0; border-radius: 5px; }}
                    .footer {{ color: #7f8c8d; font-size: 12px; margin-top: 20px; }}
                </style>
            </head>
            <body>
                <div class='header'>
                    <h2>🗓️ Appointment Reminder</h2>
                </div>
                <p>Dear {appointment.User.Name ?? "Valued User"},</p>
                <p>This is a friendly reminder that you have an upcoming appointment:</p>
                
                <div class='details'>
                    <strong>📋 Title:</strong> {appointment.Name}<br/>
                    <strong>📅 Date & Time:</strong> {appointment.StartsAt:dddd, MMMM dd, yyyy 'at' h:mm tt}<br/>
                    <strong>⏱️ EndsAt:</strong> {appointment.EndsAt} minutes
                </div>
                
                <p>Please make sure to be on time. Otherwise you'll lose your stake.</p>
                
                <div class='footer'>
                    <p>Best regards,<br/>Your Calendar App Team</p>
                    <p><em>This is an automated reminder. Please do not reply to this email.</em></p>
                </div>
            </body>
            </html>";
        }
    }
}
