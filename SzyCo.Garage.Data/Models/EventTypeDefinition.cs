namespace SzyCo.Garage.Data.Models;

[Create(nameof(Permission.UserAdmin))]
[Edit(nameof(Permission.UserAdmin))]
[Delete(nameof(Permission.UserAdmin))]
public class EventTypeDefinition
{
    [Key]
    public int EventTypeDefinitionId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string JsonDefinition { get; set; } = "{}";

    public bool IsActive { get; set; } = true;
}
