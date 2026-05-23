using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Models;
using SzyCo.Garage.Data.Auth;
using System.Security.Claims;

namespace SzyCo.Garage.Data.Test;

public class EventOwnershipTests : TestBase
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

    private Car CreateCar(string userId)
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
            Make = "Toyota",
            Model = "Camry",
            Color = "Blue",
        };
        Db.Cars.Add(car);
        Db.SaveChanges();
        return car;
    }

    private EventTypeDefinition CreateEventType()
    {
        var eventType = new EventTypeDefinition
        {
            Name = "Replacement",
            Description = "A part replacement",
            IsActive = true,
        };
        Db.EventTypeDefinitions.Add(eventType);
        Db.SaveChanges();
        return eventType;
    }

    [Fact]
    public void EventBehaviors_CreateDeniedForOtherUsersCar()
    {
        // Arrange
        var otherUsersCar = CreateCar(UserBId);
        CreateEventType();
        RefreshServices();

        SetCurrentUser(UserAId);
        var behaviors = Mocker.CreateInstance<Event.EventBehaviors>();
        var newEvent = new Event
        {
            CarId = otherUsersCar.CarId,
            EventTypeId = 1,
            JsonData = "{}",
        };

        // Act
        var result = behaviors.BeforeSave(SaveKind.Create, null, newEvent);

        // Assert
        result.AssertError("You can only add events to your own cars.");
    }

    [Fact]
    public void EventBehaviors_CreateAllowedForOwnCar()
    {
        // Arrange
        var ownCar = CreateCar(UserAId);
        CreateEventType();
        RefreshServices();

        SetCurrentUser(UserAId);
        var behaviors = Mocker.CreateInstance<Event.EventBehaviors>();
        var newEvent = new Event
        {
            CarId = ownCar.CarId,
            EventTypeId = 1,
            JsonData = "{}",
        };

        // Act
        var result = behaviors.BeforeSave(SaveKind.Create, null, newEvent);

        // Assert
        result.AssertSuccess();
    }

    [Fact]
    public void EventBehaviors_DeleteDeniedForOtherUsersCar()
    {
        // Arrange
        var otherUsersCar = CreateCar(UserBId);
        CreateEventType();
        RefreshServices();

        SetCurrentUser(UserAId);
        var behaviors = Mocker.CreateInstance<Event.EventBehaviors>();
        var existingEvent = new Event
        {
            CarId = otherUsersCar.CarId,
            EventTypeId = 1,
            JsonData = "{}",
        };

        // Act
        var result = behaviors.BeforeDelete(existingEvent);

        // Assert
        result.AssertError("You can only delete events for your own cars.");
    }

    [Fact]
    public void MyEvents_ReturnsOnlyCurrentUsersEvents()
    {
        // Arrange
        var userACar = CreateCar(UserAId);
        var userBCar = CreateCar(UserBId);
        var eventType = CreateEventType();

        Db.Events.Add(new Event { CarId = userACar.CarId, EventTypeId = eventType.EventTypeDefinitionId, JsonData = "{}" });
        Db.Events.Add(new Event { CarId = userBCar.CarId, EventTypeId = eventType.EventTypeDefinitionId, JsonData = "{}" });
        Db.SaveChanges();
        RefreshServices();

        SetCurrentUser(UserAId);
        var dataSource = Mocker.CreateInstance<Event.MyEvents>();

        // Act
        var result = dataSource.GetQuery(null!).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(userACar.CarId, result[0].CarId);
    }
}
