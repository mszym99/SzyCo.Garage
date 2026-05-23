using Microsoft.AspNetCore.Mvc;
using Moq;
using SzyCo.Garage.Web;

namespace SzyCo.Garage.Data.Test;

public class ReturnUrlHelperTests
{
    [Theory]
    [InlineData("/", true, "/")]
    [InlineData("/garage", true, "/garage")]
    [InlineData("https://example.com", false, "/")]
    [InlineData("", false, "/")]
    public void GetLocalOrDefault_ReturnsExpectedDestination(string? returnUrl, bool isLocalUrl, string expected)
    {
        var urlHelper = new Mock<IUrlHelper>();
        urlHelper.Setup(x => x.IsLocalUrl(returnUrl)).Returns(isLocalUrl);

        var destination = ReturnUrlHelper.GetLocalOrDefault(urlHelper.Object, returnUrl);

        Assert.Equal(expected, destination);
    }

    [Fact]
    public void GetLocalOrDefault_ReturnsDefaultForNull()
    {
        var urlHelper = new Mock<IUrlHelper>();

        var destination = ReturnUrlHelper.GetLocalOrDefault(urlHelper.Object, null);

        Assert.Equal("/", destination);
        urlHelper.Verify(x => x.IsLocalUrl(It.IsAny<string>()), Times.Never);
    }
}
