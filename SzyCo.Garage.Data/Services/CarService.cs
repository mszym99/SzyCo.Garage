namespace SzyCo.Garage.Data.Services;

public interface ICarService
{
    Task AddCarAsync(Car car);
}

public class CarService : ICarService
{
    private readonly AppDbContext _context;

    public CarService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddCarAsync(Car car)
    {
        if (car == null) throw new ArgumentNullException(nameof(car));

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }
}
