using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SzyCo.Garage.Data.Communication;

namespace SzyCo.Garage.Data.Test;

public class SmtpEmailServiceTests
{
    private const string PickupDirectoryTestFolder = "SzyCoGarageEmailTests";

    [Fact]
    public async Task SendEmailAsync_ReturnsFailureWhenDisabled()
    {
        var service = new SmtpEmailService(
            Options.Create(new SmtpEmailOptions
            {
                Enabled = false,
                FromAddress = "noreply@example.com",
                Host = "localhost"
            }),
            NullLogger<SmtpEmailService>.Instance);

        var result = await service.SendEmailAsync("user@example.com", "Test", "<p>Body</p>");

        Assert.False(result.WasSuccessful);
    }

    [Fact]
    public async Task SendEmailAsync_WritesEmailToPickupDirectory()
    {
        var pickupDirectory = Path.Combine(Path.GetTempPath(), PickupDirectoryTestFolder, Guid.NewGuid().ToString("N"));
        try
        {
            var service = new SmtpEmailService(
                Options.Create(new SmtpEmailOptions
                {
                    Enabled = true,
                    FromAddress = "noreply@example.com",
                    FromName = "SzyCo Garage Test",
                    PickupDirectoryLocation = pickupDirectory
                }),
                NullLogger<SmtpEmailService>.Instance);

            var result = await service.SendEmailAsync("user@example.com", "Test Subject", "<p>Body</p>");

            Assert.True(result.WasSuccessful);
            var files = Directory.GetFiles(pickupDirectory);
            Assert.Single(files);
            var mailContent = await File.ReadAllTextAsync(files[0]);
            Assert.Contains("Subject: Test Subject", mailContent);
        }
        finally
        {
            if (Directory.Exists(pickupDirectory))
            {
                Directory.Delete(pickupDirectory, recursive: true);
            }
        }
    }
}
