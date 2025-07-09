using AppointmentBooking.src.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace AppointmentBooking.src.Infrastructure.Email;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;

    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    }

    // Main method that sends the email
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _config["EmailSettings:SmtpServer"],
            int.Parse(_config["EmailSettings:Port"]),
            false // true = SSL, false = TLS
        );

        await client.AuthenticateAsync(
            _config["EmailSettings:Username"],
            _config["EmailSettings:Password"]
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    // ✅ Request model for passing email data (nested but accessible)
    public class SendEmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
