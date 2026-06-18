using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SzyCo.Garage.Data.Communication;

namespace SzyCo.Garage.Data.Test;

public class SmtpEmailServiceTests
{
    [Fact]
    public async Task SendEmailAsync_WithPickupDirectory_WritesEmailFile()
    {
        var pickupDirectory = Path.Combine(Path.GetTempPath(), "SzyCo.Garage.Tests", Guid.NewGuid().ToString("N"));
        try
        {
            var options = Options.Create(new EmailServiceOptions
            {
                FromAddress = "noreply@szyco.garage",
                PickupDirectory = pickupDirectory
            });

            var service = new SmtpEmailService(options, NullLogger<SmtpEmailService>.Instance);

            var result = await service.SendEmailAsync("user@example.com", "Confirm your email", "<p>Test</p>");

            Assert.True(result.WasSuccessful);
            var files = Directory.GetFiles(pickupDirectory);
            Assert.Single(files);
            var content = await File.ReadAllTextAsync(files[0]);
            Assert.Contains("Confirm your email", content);
        }
        finally
        {
            if (Directory.Exists(pickupDirectory))
            {
                Directory.Delete(pickupDirectory, recursive: true);
            }
        }
    }

    [Fact]
    public async Task SendEmailAsync_WithoutDeliveryConfig_ReturnsFailure()
    {
        var options = Options.Create(new EmailServiceOptions
        {
            FromAddress = "noreply@szyco.garage"
        });

        var service = new SmtpEmailService(options, NullLogger<SmtpEmailService>.Instance);

        var result = await service.SendEmailAsync("user@example.com", "Confirm your email", "<p>Test</p>");

        Assert.False(result.WasSuccessful);
        Assert.Equal("Email delivery is not configured.", result.Message);
    }
}
