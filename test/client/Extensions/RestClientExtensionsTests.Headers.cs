using BlazorFocused.Tools.Model;
using Bogus;
using Xunit;

namespace BlazorFocused.Extensions;
public partial class RestClientExtensionsTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldReturnResponseHeaders(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        var request = RestClientTestExtensions.GenerateResponseObjects();
        var successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        var expectedResponse = RestClientTestExtensions.GenerateResponseObject();
        var keyOne = "X-IPv6-Address";
        var valueOneA = new Faker().Internet.Ipv6();
        var valueOneB = new Faker().Internet.Ipv6();
        var keyTwo = "X-IPv4-Address";
        var valueTwo = new Faker().Internet.Ip();

        simulatedHttp.AddResponseHeader(keyOne, valueOneA);
        simulatedHttp.AddResponseHeader(keyOne, valueOneB);
        simulatedHttp.AddResponseHeader(keyTwo, valueTwo);

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        var actualClientResponse =
            await MakeTryRequest<SimpleClass>(httpMethod, url, request);

        var firstKeyExists = actualClientResponse.Headers.TryGetValues(keyOne, out var firstValueSet);
        var secondKeyExists = actualClientResponse.Headers.TryGetValues(keyTwo, out var secondValueSet);

        Assert.True(firstKeyExists);
        Assert.True(secondKeyExists);

        Assert.Equal(2, firstValueSet.Count());
        Assert.Single(secondValueSet);

        Assert.True(firstValueSet.Contains(valueOneA) && firstValueSet.Contains(valueOneB));
        Assert.Equal(valueTwo, secondValueSet.FirstOrDefault());
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldReturnTaskHeaders(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        var request = RestClientTestExtensions.GenerateResponseObjects();
        var successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        var expectedResponse = RestClientTestExtensions.GenerateResponseObject();
        var keyOne = "X-IPv6-Address";
        var valueOneA = new Faker().Internet.Ipv6();
        var valueOneB = new Faker().Internet.Ipv6();
        var keyTwo = "X-IPv4-Address";
        var valueTwo = new Faker().Internet.Ip();

        simulatedHttp.AddResponseHeader(keyOne, valueOneA);
        simulatedHttp.AddResponseHeader(keyOne, valueOneB);
        simulatedHttp.AddResponseHeader(keyTwo, valueTwo);

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        var actualTask = await MakeTryTaskRequest(httpMethod, url, request);

        actualTask.Headers.TryGetValues(keyOne, out var firstValueSet);
        actualTask.Headers.TryGetValues(keyTwo, out var secondValueSet);

        Assert.Equal(2, firstValueSet.Count());
        Assert.Single(secondValueSet);

        Assert.True(firstValueSet.Contains(valueOneA) && firstValueSet.Contains(valueOneB));
        Assert.Equal(valueTwo, secondValueSet.FirstOrDefault());
    }
}
