namespace Core.TrackedDomainService;

public interface ITrackedDomainService
{
    List<string> TrackedDomains { get; }
    bool IsTrackedDomain(string url);
}