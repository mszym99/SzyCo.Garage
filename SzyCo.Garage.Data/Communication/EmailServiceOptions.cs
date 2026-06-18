namespace SzyCo.Garage.Data.Communication;

public class EmailServiceOptions
{
    public const string SectionName = "Communication:Email";

    public string? FromAddress { get; set; }
    public string? FromName { get; set; }
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PickupDirectory { get; set; }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(FromAddress) &&
        (
            !string.IsNullOrWhiteSpace(PickupDirectory) ||
            !string.IsNullOrWhiteSpace(SmtpHost)
        );
}
