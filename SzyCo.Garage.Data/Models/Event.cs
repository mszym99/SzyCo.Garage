namespace SzyCo.Garage.Data.Models;

public class Event
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int EventTypeId { get; set; }
    public string JsonData { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
}
