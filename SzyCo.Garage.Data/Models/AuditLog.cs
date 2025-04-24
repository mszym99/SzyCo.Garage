using IntelliTect.Coalesce.AuditLogging;

namespace SzyCo.Garage.Data.Models;

[Edit(DenyAll)]
[Delete(DenyAll)]
[Create(DenyAll)]
[Read(nameof(Permission.ViewAuditLogs))]
public class AuditLog : DefaultAuditLog
{
    public string? UserId { get; set; }

    [Display(Name = "Changed By")]
    public User? User { get; set; }

}