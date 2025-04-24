namespace SzyCo.Garage.Data.Communication;

public interface IEmailService
{
    Task<ItemResult> SendEmailAsync(string to, string subject, string htmlMessage);
}
