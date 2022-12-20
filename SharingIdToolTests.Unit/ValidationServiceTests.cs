using Core.TrackedDomainService;
using Core.ValidationService;
using Moq;

namespace Tests.Unit;

public class ValidationServiceTests
{
    private readonly IValidationService _validationService;
    
    public ValidationServiceTests()
    {
        var trackedDomainService = new Mock<ITrackedDomainService>();
        // Most of the time, we want to return true for IsTrackedDomain
        // If not, we can override it in the test
        trackedDomainService.Setup(x => x.IsTrackedDomain(It.IsAny<string>())).Returns(true);
        _validationService = new ValidationService(trackedDomainService.Object);
    }
    
    [Fact]
    public void ShouldRunSuccessfulValidation()
    {
        // Arrange
        const string uri = "https://www.microsoft.com";
        const string id = "1234567890";
        const bool expectedGeneralStatus = true;
        var expectedStatusMessages = new List<(string message, string status)>
        {
            (Constants.ValidationMessage.UrlGeneratedSuccessfully, Constants.ValidationStatus.Success)
        };

        // Act
        _validationService.RunValidation(uri, id);

        // Assert
        Assert.Equal(expectedGeneralStatus, _validationService.IsValidationSuccessful);
        Assert.Single(_validationService.StatusMessages);
        Assert.Equal(expectedStatusMessages[0].message, _validationService.StatusMessages[0].message);
        Assert.Equal(expectedStatusMessages[0].status, _validationService.StatusMessages[0].status);
    }

    [Fact]
    public void ShouldHaveErrorDueToWrongProtocol()
    {
        // Arrange
        const string uri = "ftp://www.microsoft.com";
        const string id = "123456789";
        const bool expectedGeneralStatus = false;
        var expectedStatusMessages = new List<(string message, string status)>
        {
            (Constants.ValidationMessage.UrlSchemeNotValid, Constants.ValidationStatus.Error)
        };

        // Act
        _validationService.RunValidation(uri, id);

        // Assert
        Assert.Equal(expectedGeneralStatus, _validationService.IsValidationSuccessful);
        Assert.Single(_validationService.StatusMessages);
        Assert.Equal(expectedStatusMessages[0].message, _validationService.StatusMessages[0].message);
        Assert.Equal(expectedStatusMessages[0].status, _validationService.StatusMessages[0].status);
    }

    [Fact]
    public void ShouldHaveErrorDueToWrongUrlFormat()
    {
        // Arrange
        const string uri = "invalidUri";
        const string id = "123456789";
        const bool expectedGeneralStatus = false;
        var expectedStatusMessages = new List<(string message, string status)>
        {
            (Constants.ValidationMessage.UrlNotValid, Constants.ValidationStatus.Error)
        };

        // Act
        _validationService.RunValidation(uri, id);

        // Assert
        Assert.Equal(expectedGeneralStatus, _validationService.IsValidationSuccessful);
        Assert.Single(_validationService.StatusMessages);
        Assert.Equal(expectedStatusMessages[0].message, _validationService.StatusMessages[0].message);
        Assert.Equal(expectedStatusMessages[0].status, _validationService.StatusMessages[0].status);
    }

    [Fact]
    public void ShouldHaveWarningDueToWrongDomain()
    {
        // Arrange
        const string uri = "https://www.microsoft.com";
        const string id = "123456789";
        const bool expectedGeneralStatus = true;
        const int expectedNumberOfStatusMessages = 2;
        var expectedStatusMessages = new List<(string message, string status)>
        {
            (Constants.ValidationMessage.UrlGeneratedWithWarnings, Constants.ValidationStatus.Warning),
            (Constants.ValidationMessage.NotTrackedDomain, Constants.ValidationStatus.Warning)
        };

        // Create custom validationService & mock for ITrackedDomainService to return false for IsTrackedDomain
        var trackedDomainService = new Mock<ITrackedDomainService>();
        trackedDomainService.Setup(x => x.IsTrackedDomain(It.IsAny<string>())).Returns(false);
        var validationService = new ValidationService(trackedDomainService.Object);

        // Act
        validationService.RunValidation(uri, id);

        // Assert
        Assert.Equal(expectedGeneralStatus, validationService.IsValidationSuccessful);
        Assert.Equal(expectedNumberOfStatusMessages, validationService.StatusMessages.Count);
        Assert.Equal(expectedStatusMessages[0].message, validationService.StatusMessages[0].message);
        Assert.Equal(expectedStatusMessages[0].status, validationService.StatusMessages[0].status);
        Assert.Equal(expectedStatusMessages[1].message, validationService.StatusMessages[1].message);
        Assert.Equal(expectedStatusMessages[1].status, validationService.StatusMessages[1].status);
    }

    [Fact]
    public void ShouldResetInternalStateBetweenRuns()
    {
        // It is somewhat tested by having one instance of ValidationService in the constructor,
        // but we can also test it explicitly
        // Arrange
        const string uri1 = "https://www.microsoft.com";
        const string uri2 = "invalidUri";
        const string id = "123456789";
        const bool expectedGeneralStatus1 = true;
        const bool expectedGeneralStatus2 = false;
        var expectedStatusMessages1 = new List<(string message, string status)>
        {
            (Constants.ValidationMessage.UrlGeneratedSuccessfully, Constants.ValidationStatus.Success)
        };
        var expectedStatusMessages2 = new List<(string message, string status)>
        {
            (Constants.ValidationMessage.UrlNotValid, Constants.ValidationStatus.Error)
        };

        // Act 1
        _validationService.RunValidation(uri1, id);

        // Assert 1
        Assert.Equal(expectedGeneralStatus1, _validationService.IsValidationSuccessful);
        Assert.Single(_validationService.StatusMessages);
        Assert.Equal(expectedStatusMessages1[0].message, _validationService.StatusMessages[0].message);
        Assert.Equal(expectedStatusMessages1[0].status, _validationService.StatusMessages[0].status);

        // Act 2
        _validationService.RunValidation(uri2, id);

        // Assert 2
        Assert.Equal(expectedGeneralStatus2, _validationService.IsValidationSuccessful);
        Assert.Single(_validationService.StatusMessages);
        Assert.Equal(expectedStatusMessages2[0].message, _validationService.StatusMessages[0].message);
        Assert.Equal(expectedStatusMessages2[0].status, _validationService.StatusMessages[0].status);
    }
}