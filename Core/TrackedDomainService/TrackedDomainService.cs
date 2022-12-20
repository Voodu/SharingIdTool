namespace Core.TrackedDomainService;

public class TrackedDomainService : ITrackedDomainService
{
    public List<string> TrackedDomains { get; } = new()
    {
        "www.docs.microsoft.com",
        "www.learn.microsoft.com",
        "www.devblogs.microsoft.com",
        "docs.microsoft.com",
        "learn.microsoft.com",
        "code.visualstudio.com/docs",
        "azure.microsoft.com",
        "techcommunity.microsoft.com",
        "social.technet.microsoft.com",
        "social.msdn.microsoft.com",
        "devblogs.microsoft.com",
        "developer.microsoft.com",
        "cloudblogs.microsoft.com"
    };

    public bool IsTrackedDomain(string url)
    {
        var tempLink = new Uri(url.ToLower());
        var host = tempLink.Host;
        // If it's code.visualstudio.com, we need to check the host and first segment
        if (host == "code.visualstudio.com" && tempLink.Segments.Length > 1)
        {
            host = $"{host}/{tempLink.Segments[1]}";
        }
        return TrackedDomains.Contains(host);
    }
}