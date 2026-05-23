namespace SzyCo.Garage.Data.Models;

public class EventTypeDefinition
{
    [Key]
    public int EventTypeDefinitionId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
