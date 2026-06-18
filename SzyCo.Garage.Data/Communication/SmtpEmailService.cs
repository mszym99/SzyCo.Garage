using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace SzyCo.Garage.Data.Communication;

public class SmtpEmailService(
    IOptions<EmailServiceOptions> optionsAccessor,
    ILogger<SmtpEmailService> logger
) : IEmailService
{
    public async Task<ItemResult> SendEmailAsync(string to, string subject, string htmlMessage)
    {
        var options = optionsAccessor.Value;

        if (string.IsNullOrWhiteSpace(options.FromAddress))
        {
            return "Email sender address is not configured.";
        }

        if (string.IsNullOrWhiteSpace(options.SmtpHost) && string.IsNullOrWhiteSpace(options.PickupDirectory))
        {
            return "Email delivery is not configured.";
        }

        try
        {
            using var message = new MailMessage(
                new MailAddress(options.FromAddress, options.FromName),
                new MailAddress(to)
            )
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            using var smtpClient = BuildSmtpClient(options);
            await smtpClient.SendMailAsync(message);

            return new ItemResult(true, "Email sent.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Recipient}", to);
            return new ItemResult(false, "Unable to send email right now.");
        }
    }

    private static SmtpClient BuildSmtpClient(EmailServiceOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.PickupDirectory))
        {
            Directory.CreateDirectory(options.PickupDirectory);
            return new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = options.PickupDirectory
            };
        }

        var client = new SmtpClient(options.SmtpHost!, options.SmtpPort)
        {
            EnableSsl = options.UseSsl
        };

        if (!string.IsNullOrWhiteSpace(options.Username))
        {
            client.Credentials = new NetworkCredential(options.Username, options.Password);
        }

        return client;
    }
}
