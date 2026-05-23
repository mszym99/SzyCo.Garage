namespace SzyCo.Garage.Data.Test;

public class WidgetTests : TestBase
{
    [Fact]
    public void Widget_CanCreateAndRetrieve()
    {
        // Arrange
        var widget1 = new Widget { Name = "Gnoam Sprecklesprocket", Category = WidgetCategory.Sprecklesprockets };
        Db.Add(widget1);
        Db.SaveChanges();

        RefreshServices();

        // Act
        var widget2 = Db.Widgets.Single();

        // Assert
        Assert.Equal(WidgetCategory.Sprecklesprockets, widget2.Category);
        Assert.NotEqual(widget1, widget2);
    }
}