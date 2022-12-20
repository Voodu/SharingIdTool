using Core;
using Core.MergerService;

namespace Tests.Unit;

public class MergerServiceTests
{
    [Theory]
    [InlineData(
        "https://docs.microsoft.com",
        "studentamb_12345",
        "https://docs.microsoft.com/?wt.mc_id=studentamb_12345")]
    [InlineData(
        "https://docs.microsoft.com/",
        "studentamb_12345",
        "https://docs.microsoft.com/?wt.mc_id=studentamb_12345")]
    [InlineData(
        "https://docs.microsoft.com/azure/azure-functions/functions-bindings-storage-table?tabs=csharp",
        "studentamb_12345",
        "https://docs.microsoft.com/azure/azure-functions/functions-bindings-storage-table?tabs=csharp&wt.mc_id=studentamb_12345")]
    [InlineData(
        "https://docs.microsoft.com/en-us",
        "studentamb_12345",
        "https://docs.microsoft.com/?wt.mc_id=studentamb_12345")]
    [InlineData(
        "https://docs.microsoft.com/en-us/",
        "studentamb_12345",
        "https://docs.microsoft.com/?wt.mc_id=studentamb_12345")]
    [InlineData(
        "https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table?tabs=csharp",
        "studentamb_12345",
        "https://docs.microsoft.com/azure/azure-functions/functions-bindings-storage-table?tabs=csharp&wt.mc_id=studentamb_12345")]
    [InlineData(
        "https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table?tabs=csharp/",
        "studentamb_12345",
        "https://docs.microsoft.com/azure/azure-functions/functions-bindings-storage-table?tabs=csharp&wt.mc_id=studentamb_12345")]
    [InlineData(
        "https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table?tabs=csharp#next-steps",
        "studentamb_12345",
        "https://docs.microsoft.com/azure/azure-functions/functions-bindings-storage-table?tabs=csharp&wt.mc_id=studentamb_12345#next-steps")]
    public void ShouldHandleAllCases(string url, string msaId, string expectedUrl)
    {
        // Arrange
        var mergerService = new MergerService();

        // Act
        var result = mergerService.Merge(url, msaId);

        // Assert
        Assert.Equal(expectedUrl, result.ToString());
    }
}