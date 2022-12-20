namespace Core.ValidationService;

public static partial class Constants
{
    public static class ValidationStatus
    {
        public const string Error = "error";
        public const string Warning = "warning";
        public const string Success = "success";
    }

    public static class ValidationMessage
    {
        public const string UrlNotValid = "The URL is not valid.";
        public const string UrlSchemeNotValid = "URL can only be HTTP or HTTPS.";
        public const string NotTrackedDomain = "Not a tracked domain.";
        public const string UrlGeneratedSuccessfully = "Everything is fine. Copy your URL to the clipboard.";
        public const string UrlGeneratedWithWarnings = "URL was generated, but there are some issues:";
    }
}