using BlazorFocused.Tools.Utility;
using Bogus;
using System.Net;
using Xunit;

namespace BlazorFocused.Tools.Http;

public partial class SimulatedHttpTests
{
    [Fact]
    public async Task ShouldTrackRequestHeaders()
    {
        var httpMethod = HttpMethod.Get;
        var url = GetRandomRelativeUrl();
        var response = GetRandomSimpleClass();
        var key = new Faker().Random.AlphaNumeric(5);
        var expectedValue = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, response);

        using var httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        testService.AddRequestHeader(key, expectedValue);

        _ = await testService.MakeRequestAsync(httpMethod, url);

        // Under Test
        var actualValue = simulatedHttp.GetRequestHeaderValues(httpMethod, url, key).FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task ShouldTrackDefaultHeaders()
    {
        var httpMethod = HttpMethod.Get;
        var url = GetRandomRelativeUrl();
        var response = GetRandomSimpleClass();
        var key = new Faker().Random.AlphaNumeric(5);
        var expectedValue = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, response);

        using var httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        testService.AddDefaultHeader(key, expectedValue);

        _ = await testService.MakeRequestAsync(httpMethod, url);

        // Under Test
        var actualValue = simulatedHttp.GetRequestHeaderValues(httpMethod, url, key).FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task ShouldSetResponseHeaders()
    {
        var httpMethod = HttpMethod.Get;
        var url = GetRandomRelativeUrl();
        var key = new Faker().Random.AlphaNumeric(5);
        var expectedValue = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, GetRandomSimpleClass());

        // Under Test
        simulatedHttp.AddResponseHeader(key, expectedValue);

        using var httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        var responseMesssage = await testService.MakeRequestAsync(httpMethod, url);

        var responseHeaderExists =
            responseMesssage.Headers.TryGetValues(key, out var values);

        Assert.True(responseHeaderExists);
        Assert.Single(values);
        Assert.Equal(expectedValue, values.FirstOrDefault());
    }

    [Fact]
    public async Task ShouldSetMultipleResponseHeadersWithSameKey()
    {
        var httpMethod = HttpMethod.Get;
        var url = GetRandomRelativeUrl();
        var key = new Faker().Random.AlphaNumeric(5);
        var expectedValueOne = new Faker().Random.AlphaNumeric(10);
        var expectedValueTwo = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, GetRandomSimpleClass());

        // Under Test
        simulatedHttp.AddResponseHeader(key, expectedValueOne);
        simulatedHttp.AddResponseHeader(key, expectedValueTwo);

        using var httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        var responseMesssage = await testService.MakeRequestAsync(httpMethod, url);

        var responseHeaderExists =
            responseMesssage.Headers.TryGetValues(key, out var values);

        Assert.True(responseHeaderExists);
        Assert.Equal(2, values.Count());
        Assert.True(values.Contains(expectedValueOne) && values.Contains(expectedValueTwo));
    }
}
