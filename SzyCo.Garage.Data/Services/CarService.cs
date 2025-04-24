using Microsoft.EntityFrameworkCore;
using SzyCo.Cars.Data;
using SzyCo.Cars.Data.Models;
using SzyCo.Garage.Data;

[Service]
[Coalesce]
public class CarService
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
