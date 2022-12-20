using Core.TrackedDomainService;
using ValidationMessage = Core.ValidationService.Constants.ValidationMessage;
using ValidationStatus = Core.ValidationService.Constants.ValidationStatus;

namespace Core.ValidationService;

public class ValidationService : IValidationService
{
    private readonly ITrackedDomainService _trackedDomainService;

    public ValidationService(ITrackedDomainService trackedDomainService)
    {
        _trackedDomainService = trackedDomainService;
        StatusMessages = new List<(string message, string status)>();
    }

    public bool IsValidationSuccessful { get; private set; }
    public List<(string message, string status)> StatusMessages { get; }

    public void RunValidation(string url, string id)
    {
        ResetValidation();

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            StatusMessages.Add((ValidationMessage.UrlNotValid, ValidationStatus.Error));
            // Terminate immediately. Nothing can be done if the URL is invalid
            IsValidationSuccessful = false;
            return;
        }

        if (!HasUriValidScheme(uri))
        {
            StatusMessages.Add((ValidationMessage.UrlSchemeNotValid, ValidationStatus.Error));
            // Terminate immediately. Generating URL with weird protocol might only confuse the user
            IsValidationSuccessful = false;
            return;
        }

        if (!_trackedDomainService.IsTrackedDomain(url))
        {
            StatusMessages.Add((ValidationMessage.NotTrackedDomain, ValidationStatus.Warning));
        }

        if (StatusMessages.Count > 0)
        {
            // Insert general info at the beginning, so it's displayed at the top
            StatusMessages.Insert(0, (ValidationMessage.UrlGeneratedWithWarnings, ValidationStatus.Warning));
        }
        else
        {
            StatusMessages.Add((ValidationMessage.UrlGeneratedSuccessfully, ValidationStatus.Success));
        }
    }

    private void ResetValidation()
    {
        StatusMessages.Clear();
        IsValidationSuccessful = true;
    }

    private static bool HasUriValidScheme(Uri uri)
    {
        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }
}