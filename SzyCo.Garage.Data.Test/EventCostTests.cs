namespace SzyCo.Garage.Data.Test;

public class EventCostTests
{
    [Theory]
    [InlineData("""{"cost":"125.50"}""", 125.50)]
    [InlineData("""{"Cost":"$1,200"}""", 1200)]
    [InlineData("""{"cost":25.75}""", 25.75)]
    [InlineData("""{"reason":"No cost"}""", 0)]
    [InlineData("not json", 0)]
    [InlineData(null, 0)]
    public void GetCostFromJsonData_ReturnsParsedCost(string? jsonData, decimal expected)
    {
        var result = Event.GetCostFromJsonData(jsonData);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Car_TotalEventHistoryCost_SumsEventCosts()
    {
        var car = new Car
        {
            UserId = "user-id",
            Year = 2024,
            Make = "Toyota",
            Model = "Camry",
            Color = "Blue",
            Events =
            [
                new Event { JsonData = """{"cost":"100.50"}""" },
                new Event { JsonData = """{"cost":25}""" },
                new Event { JsonData = """{"reason":"No cost"}""" },
            ],
        };

        Assert.Equal(125.50m, car.TotalEventHistoryCost);
    }
}
