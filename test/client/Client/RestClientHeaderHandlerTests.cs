using BlazorFocused.Tools;
using Bogus;
using Xunit;

namespace BlazorFocused.Client;
public class RestClientHeaderHandlerTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ITestLogger<RestClientHeaderHandler> testLogger;
    private readonly string baseAddress = new Faker().Internet.Url();

    public RestClientHeaderHandlerTests()
    {
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        testLogger = ToolsBuilder.CreateTestLogger<RestClientHeaderHandler>(); ;
    }

    [Fact]
    public async Task ShouldAddAuthTokenToRequest()
    {
        var keyOne = "X-IPv6-Address";
        var valueOneA = new Faker().Internet.Ipv6();
        var valueOneB = new Faker().Internet.Ipv6();
        var keyTwo = "X-IPv4-Address";
        var valueTwo = new Faker().Internet.Ip();
        var relativePath = new Faker().Internet.UrlRootedPath();

        var restClientRequestHeaders = new RestClientRequestHeaders
        {
            Enabled = true,
            HeaderCache = new Dictionary<string, List<string>>()
        };

        simulatedHttp.SetupGET(relativePath)
            .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

        var restClientAuthHandler = new RestClientHeaderHandler(restClientRequestHeaders, testLogger)
        {
            InnerHandler = simulatedHttp.DelegatingHandler
        };

        using var httpClient = new HttpClient(restClientAuthHandler)
        {
            BaseAddress = new Uri(baseAddress)
        };

        var httpResponseMessage = await httpClient.GetAsync(relativePath);

        httpResponseMessage.EnsureSuccessStatusCode();

        var firstKeyExists = httpResponseMessage.Headers.TryGetValues(keyOne, out var firstValueSet);
        var secondKeyExists = httpResponseMessage.Headers.TryGetValues(keyTwo, out var secondValueSet);

        Assert.True(firstKeyExists);
        Assert.True(secondKeyExists);

        Assert.Equal(2, firstValueSet.Count());
        Assert.Single(secondValueSet);

        Assert.True(firstValueSet.Contains(valueOneA) && firstValueSet.Contains(valueOneB));
        Assert.Equal(valueTwo, secondValueSet.FirstOrDefault());
    }
}
