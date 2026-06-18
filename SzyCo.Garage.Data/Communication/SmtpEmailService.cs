using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace SzyCo.Garage.Data.Communication;

public class SmtpEmailService(
    IOptions<SmtpEmailOptions> options,
    ILogger<SmtpEmailService> logger
) : IEmailService
{
    private readonly SmtpEmailOptions _options = options.Value;

    public async Task<ItemResult> SendEmailAsync(string to, string subject, string htmlMessage)
    {
        if (!_options.Enabled)
        {
            return new ItemResult(false, "Email sending is disabled by configuration.");
        }

        if (string.IsNullOrWhiteSpace(_options.FromAddress))
        {
            return new ItemResult(false, "Email sending is not configured: missing sender address.");
        }

        var isPickupMode = !string.IsNullOrWhiteSpace(_options.PickupDirectoryLocation);
        if (!isPickupMode && string.IsNullOrWhiteSpace(_options.Host))
        {
            return new ItemResult(false, "Email sending is not configured: missing SMTP host.");
        }

        try
        {
            using var message = new MailMessage
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                From = string.IsNullOrWhiteSpace(_options.FromName)
                    ? new MailAddress(_options.FromAddress)
                    : new MailAddress(_options.FromAddress, _options.FromName),
            };

            message.To.Add(to);

            using var client = new SmtpClient();

            if (isPickupMode)
            {
                Directory.CreateDirectory(_options.PickupDirectoryLocation!);
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = _options.PickupDirectoryLocation;
            }
            else
            {
                client.Host = _options.Host;
                client.Port = _options.Port;
                client.EnableSsl = _options.EnableSsl;

                if (!string.IsNullOrWhiteSpace(_options.Username))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_options.Username, _options.Password);
                }
                else
                {
                    client.UseDefaultCredentials = true;
                }
            }

            await client.SendMailAsync(message);

            return new ItemResult(true, "Email sent.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {EmailAddress}", to);
            return new ItemResult(false, "Failed to send email. Check email configuration and logs.");
        }
    }
}
