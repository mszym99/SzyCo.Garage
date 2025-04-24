using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SzyCo.Garage.Data;
using SzyCo.Garage.Data.Models;

namespace SzyCo.Cars.Data.Models;

public class Car
{
    [Key]
    public int CarId { get; set; }

    // Foreign key to AspNetUsers.Id
    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    // Navigation property to the Identity user
    public User User { get; set; }

    public required int Year { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required string Color { get; set; }

    [DefaultDataSource]
    public class MyGarage : StandardDataSource<Car, AppDbContext> {
        public MyGarage(CrudContext<AppDbContext> context) : base(context) { }

        public override IQueryable<Car> GetQuery(IDataSourceParameters parameters)
            => Db.Cars.Where(f => f.UserId == User.GetUserId());    
    }

}
