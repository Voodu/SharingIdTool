namespace Core.ValidationService;

public interface IValidationService
{
    bool IsValidationSuccessful { get; }
    List<(string message, string status)> StatusMessages { get; }
    void RunValidation(string url, string id);
}