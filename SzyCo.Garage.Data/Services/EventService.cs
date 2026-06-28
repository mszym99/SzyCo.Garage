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

    [Coalesce, Execute(HttpMethod = HttpMethod.Post)]
    public async Task<ItemResult<Event>> CopyEventToTodayAsync(ClaimsPrincipal user, int eventId)
    {
        var userId = user.GetUserId();
        if (string.IsNullOrEmpty(userId)) return "Authentication required.";

        var sourceEvent = await _context.Events
            .Include(e => e.Car)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (sourceEvent == null) return "Event not found.";
        if (sourceEvent.Car.UserId != userId) return "You can only copy events for your own cars.";

        var now = DateTime.UtcNow;
        var copiedEvent = new Event
        {
            CarId = sourceEvent.CarId,
            EventTypeId = sourceEvent.EventTypeId,
            JsonData = sourceEvent.JsonData,
            CreateDate = now,
            ModifiedDate = now,
        };

        _context.Events.Add(copiedEvent);
        await _context.SaveChangesAsync();

        return new ItemResult<Event> { Object = copiedEvent };
    }
}
