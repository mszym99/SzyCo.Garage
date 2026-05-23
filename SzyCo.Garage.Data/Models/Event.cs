namespace SzyCo.Garage.Data.Models;

public class Event
{
    public int Id { get; set; }

    [ForeignKey(nameof(Car))]
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    [ForeignKey(nameof(EventTypeDefinition))]
    public int EventTypeId { get; set; }
    public EventTypeDefinition EventTypeDefinition { get; set; } = null!;

    public string JsonData { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    [DefaultDataSource]
    public class MyEvents : AppDataSource<Event>
    {
        public MyEvents(CrudContext<AppDbContext> context) : base(context) { }

        public override IQueryable<Event> GetQuery(IDataSourceParameters parameters)
            => Db.Events
                .Include(e => e.EventTypeDefinition)
                .Include(e => e.Car)
                .Where(e => e.Car.UserId == User.GetUserId());
    }

    public class EventBehaviors : AppBehaviors<Event>
    {
        public EventBehaviors(CrudContext<AppDbContext> context) : base(context) { }

        public override ItemResult BeforeSave(SaveKind kind, Event? oldItem, Event item)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return "Authentication required.";

            var car = Db.Cars.FirstOrDefault(c => c.CarId == item.CarId);
            if (car == null) return "Car not found.";
            if (car.UserId != userId) return "You can only add events to your own cars.";

            if (kind == SaveKind.Create)
            {
                item.CreateDate = DateTime.UtcNow;
            }
            item.ModifiedDate = DateTime.UtcNow;

            return base.BeforeSave(kind, oldItem, item);
        }

        public override ItemResult BeforeDelete(Event item)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return "Authentication required.";

            var car = Db.Cars.FirstOrDefault(c => c.CarId == item.CarId);
            if (car == null || car.UserId != userId) return "You can only delete events for your own cars.";

            return base.BeforeDelete(item);
        }
    }
}
