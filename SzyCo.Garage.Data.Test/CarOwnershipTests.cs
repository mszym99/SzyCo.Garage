using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Models;
using SzyCo.Garage.Data.Auth;
using System.Security.Claims;

namespace SzyCo.Garage.Data.Test;

public class CarOwnershipTests : TestBase
{
    private const string UserAId = "user-a-id";
    private const string UserBId = "user-b-id";

    private void SetCurrentUser(string userId)
    {
        CurrentUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(AppClaimTypes.UserId, userId),
        }, "test"));
    }

    private Car CreateCar(string userId, string make = "Toyota", string model = "Camry")
    {
        // Ensure the user exists for FK constraint
        if (!Db.Users.Any(u => u.Id == userId))
        {
            Db.Users.Add(new User { Id = userId, UserName = $"user-{userId}", Email = $"{userId}@test.com" });
            Db.SaveChanges();
        }

        var car = new Car
        {
            UserId = userId,
            Year = 2024,
            Make = make,
            Model = model,
            Color = "Blue",
        };
        Db.Cars.Add(car);
        Db.SaveChanges();
        return car;
    }

    [Fact]
    public void MyGarage_ReturnsOnlyCurrentUsersCars()
    {
        // Arrange
        CreateCar(UserAId, "Toyota", "Camry");
        CreateCar(UserAId, "Honda", "Civic");
        CreateCar(UserBId, "Ford", "Mustang");
        RefreshServices();

        SetCurrentUser(UserAId);
        var dataSource = Mocker.CreateInstance<Car.MyGarage>();

        // Act
        var result = dataSource.GetQuery(null!).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(UserAId, c.UserId));
    }

    [Fact]
    public void MyGarage_DoesNotReturnOtherUsersCars()
    {
        // Arrange
        CreateCar(UserBId, "Ford", "Mustang");
        RefreshServices();

        SetCurrentUser(UserAId);
        var dataSource = Mocker.CreateInstance<Car.MyGarage>();

        // Act
        var result = dataSource.GetQuery(null!).ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void CarBehaviors_CreateSetsUserId()
    {
        // Arrange
        SetCurrentUser(UserAId);
        var behaviors = Mocker.CreateInstance<Car.CarBehaviors>();
        var newCar = new Car { UserId = "should-be-overwritten", Year = 2024, Make = "Test", Model = "Car", Color = "Red" };

        // Act
        var result = behaviors.BeforeSave(SaveKind.Create, null, newCar);

        // Assert
        result.AssertSuccess();
        Assert.Equal(UserAId, newCar.UserId);
    }

    [Fact]
    public void CarBehaviors_UpdateDeniedForOtherUsersCar()
    {
        // Arrange
        SetCurrentUser(UserAId);
        var behaviors = Mocker.CreateInstance<Car.CarBehaviors>();
        var existingCar = new Car { UserId = UserBId, Year = 2024, Make = "Test", Model = "Car", Color = "Red" };
        var updatedCar = new Car { UserId = UserBId, Year = 2025, Make = "Test", Model = "Car", Color = "Red" };

        // Act
        var result = behaviors.BeforeSave(SaveKind.Update, existingCar, updatedCar);

        // Assert
        result.AssertError("You can only edit your own cars.");
    }

    [Fact]
    public void CarBehaviors_DeleteDeniedForOtherUsersCar()
    {
        // Arrange
        SetCurrentUser(UserAId);
        var behaviors = Mocker.CreateInstance<Car.CarBehaviors>();
        var otherUsersCar = new Car { UserId = UserBId, Year = 2024, Make = "Test", Model = "Car", Color = "Red" };

        // Act
        var result = behaviors.BeforeDelete(otherUsersCar);

        // Assert
        result.AssertError("You can only delete your own cars.");
    }

    [Fact]
    public void CarBehaviors_DeleteAllowedForOwnCar()
    {
        // Arrange
        SetCurrentUser(UserAId);
        var behaviors = Mocker.CreateInstance<Car.CarBehaviors>();
        var ownCar = new Car { UserId = UserAId, Year = 2024, Make = "Test", Model = "Car", Color = "Red" };

        // Act
        var result = behaviors.BeforeDelete(ownCar);

        // Assert
        result.AssertSuccess();
    }
}
