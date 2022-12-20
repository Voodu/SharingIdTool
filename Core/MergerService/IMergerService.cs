namespace Core.MergerService;

public interface IMergerService
{
    public Uri Merge(string uriString, string msaId);
}