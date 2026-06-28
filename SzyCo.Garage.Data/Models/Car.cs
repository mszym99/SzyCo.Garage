namespace SzyCo.Garage.Data.Models;

public class Car
{
    [Key]
    public int CarId { get; set; }

    [ForeignKey(nameof(User))]
    [Read]
    public string UserId { get; set; } = null!;

    [Read]
    public User User { get; set; } = null!;

    public required int Year { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required string Color { get; set; }

    [Read]
    public bool IsArchived { get; set; }

    public ICollection<Event>? Events { get; set; }

    [NotMapped]
    [Read]
    public decimal TotalEventHistoryCost => Events?.Sum(e => e.Cost) ?? 0m;

    [DefaultDataSource]
    public class MyGarage : AppDataSource<Car>
    {
        public MyGarage(CrudContext<AppDbContext> context) : base(context) { }

        public override IQueryable<Car> GetQuery(IDataSourceParameters parameters)
            => Db.Cars
                .Include(c => c.Events)
                .Where(f => f.UserId == User.GetUserId());
    }

    public class CarBehaviors : AppBehaviors<Car>
    {
        public CarBehaviors(CrudContext<AppDbContext> context) : base(context) { }

        public override ItemResult BeforeSave(SaveKind kind, Car? oldItem, Car item)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return "Authentication required.";

            if (kind == SaveKind.Create)
            {
                // Server sets ownership — ignore any client-supplied value
                item.UserId = userId;
                item.IsArchived = false;
            }
            else
            {
                // On update, verify the car belongs to the current user
                if (oldItem?.UserId != userId) return "You can only edit your own cars.";
                if (oldItem.IsArchived) return "Sold vehicles are read-only.";
                // Prevent transferring ownership
                item.UserId = userId;
                item.IsArchived = oldItem.IsArchived;
            }

            return base.BeforeSave(kind, oldItem, item);
        }

        public override ItemResult BeforeDelete(Car item)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return "Authentication required.";
            if (item.UserId != userId) return "You can only delete your own cars.";

            var dependentEvents = Db.Events.Where(e => e.CarId == item.CarId).ToList();
            if (dependentEvents.Count > 0)
            {
                Db.Events.RemoveRange(dependentEvents);
            }

            return base.BeforeDelete(item);
        }
    }
}
