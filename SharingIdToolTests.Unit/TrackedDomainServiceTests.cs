using Core.TrackedDomainService;

namespace Tests.Unit;

public class TrackedDomainServiceTests
{
    private readonly ITrackedDomainService _trackedDomainService;

    public TrackedDomainServiceTests()
    {
        // Arrange
        _trackedDomainService = new TrackedDomainService();
    }

    [Theory]
    [InlineData("https://docs.microsoft.com", true)]
    [InlineData("https://learn.microsoft.com/en-us/samples/browse/", true)]
    [InlineData("https://code.visualstudio.com/docs#first-steps", true)]
    [InlineData("https://azure.microsoft.com/en-us/products/azure-automanage/", true)]
    [InlineData("https://techcommunity.microsoft.com/t5/custom/page/page-id/MyDashboard", true)]
    [InlineData("https://social.technet.microsoft.com/wiki", true)]
    [InlineData("https://social.msdn.microsoft.com/profile", true)]
    [InlineData("https://devblogs.microsoft.com/surface-duo/droidcon-new-york-london-2022/", true)]
    [InlineData("https://developer.microsoft.com/en-us/reactor", true)]
    [InlineData("https://cloudblogs.microsoft.com/quantum/", true)]
    public void ShouldConfirmDefaultUrls(string url, bool isTracked)
    {
        // Act
        var result = _trackedDomainService.IsTrackedDomain(url);

        // Assert
        Assert.Equal(isTracked, result);
    }

    [Theory]
    [InlineData("https://www.docs.microsoft.com", true)]
    [InlineData("https://www.learn.microsoft.com/en-us/samples/browse/", true)]
    [InlineData("https://www.devblogs.microsoft.com/surface-duo/droidcon-new-york-london-2022/", true)]
    public void ShouldConfirmValidWWWUrls(string url, bool isTracked)
    {
        // Act
        var result = _trackedDomainService.IsTrackedDomain(url);

        // Assert
        Assert.Equal(isTracked, result);
    }

    [Theory]
    [InlineData("https://www.code.visualstudio.com/docs#first-steps", false)]
    // www.azure.microsoft.com does not create a redirect for the whole link; it always redirects to the root
    [InlineData("https://www.azure.microsoft.com/en-us/products/azure-automanage/", false)]
    [InlineData("https://www.techcommunity.microsoft.com/t5/custom/page/page-id/MyDashboard", false)]
    [InlineData("https://www.social.technet.microsoft.com/wiki", false)]
    [InlineData("https://www.social.msdn.microsoft.com/profile", false)]
    [InlineData("https://www.developer.microsoft.com/en-us/reactor", false)]
    [InlineData("https://www.cloudblogs.microsoft.com/quantum/", false)]
    public void ShouldRejectInvalidWWWUrls(string url, bool isTracked)
    {
        // Act
        var result = _trackedDomainService.IsTrackedDomain(url);

        // Assert
        Assert.Equal(isTracked, result);
    }

    [Theory]
    [InlineData("https://www.google.com", false)]
    [InlineData("https://www.bing.com", false)]
    [InlineData("https://www.yahoo.com", false)]
    [InlineData("https://www.stackoverflow.com", false)]
    [InlineData("https://code.visualstudio.com", false)]
    public void ShouldRejectRandomUrls(string url, bool isTracked)
    {
        // Act
        var result = _trackedDomainService.IsTrackedDomain(url);

        // Assert
        Assert.Equal(isTracked, result);
    }
}