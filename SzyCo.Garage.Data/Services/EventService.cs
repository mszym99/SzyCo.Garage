namespace SzyCo.Garage.Data.Services;

[Coalesce]
[Service]
public class EventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddEventAsync(Event newEvent)
    {
        if (newEvent == null) throw new ArgumentNullException(nameof(newEvent));

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
    }
}
