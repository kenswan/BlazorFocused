// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

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
        HttpMethod httpMethod = HttpMethod.Get;
        string url = GetRandomRelativeUrl();
        Model.SimpleClass response = GetRandomSimpleClass();
        string key = new Faker().Random.AlphaNumeric(5);
        string expectedValue = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, response);

        using HttpClient httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        testService.AddRequestHeader(key, expectedValue);

        _ = await testService.MakeRequestAsync(httpMethod, url);

        // Under Test
        string actualValue = simulatedHttp.GetRequestHeaderValues(httpMethod, url, key).FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task ShouldTrackDefaultHeaders()
    {
        HttpMethod httpMethod = HttpMethod.Get;
        string url = GetRandomRelativeUrl();
        Model.SimpleClass response = GetRandomSimpleClass();
        string key = new Faker().Random.AlphaNumeric(5);
        string expectedValue = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, response);

        using HttpClient httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        testService.AddDefaultHeader(key, expectedValue);

        _ = await testService.MakeRequestAsync(httpMethod, url);

        // Under Test
        string actualValue = simulatedHttp.GetRequestHeaderValues(httpMethod, url, key).FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task ShouldSetResponseHeaders()
    {
        HttpMethod httpMethod = HttpMethod.Get;
        string url = GetRandomRelativeUrl();
        string key = new Faker().Random.AlphaNumeric(5);
        string expectedValue = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, GetRandomSimpleClass());

        // Under Test
        simulatedHttp.AddResponseHeader(key, expectedValue);

        using HttpClient httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        HttpResponseMessage responseMesssage = await testService.MakeRequestAsync(httpMethod, url);

        bool responseHeaderExists =
            responseMesssage.Headers.TryGetValues(key, out IEnumerable<string>? values);

        Assert.True(responseHeaderExists);
        Assert.Single(values);
        Assert.Equal(expectedValue, values.FirstOrDefault());
    }

    [Fact]
    public async Task ShouldSetMultipleResponseHeadersWithSameKey()
    {
        HttpMethod httpMethod = HttpMethod.Get;
        string url = GetRandomRelativeUrl();
        string key = new Faker().Random.AlphaNumeric(5);
        string expectedValueOne = new Faker().Random.AlphaNumeric(10);
        string expectedValueTwo = new Faker().Random.AlphaNumeric(10);

        GetHttpSetup(httpMethod, url, null)
            .ReturnsAsync(HttpStatusCode.OK, GetRandomSimpleClass());

        // Under Test
        simulatedHttp.AddResponseHeader(key, expectedValueOne);
        simulatedHttp.AddResponseHeader(key, expectedValueTwo);

        using HttpClient httpClient = simulatedHttp.HttpClient;

        var testService = new TestHttpHeaderService(httpClient);

        HttpResponseMessage responseMesssage = await testService.MakeRequestAsync(httpMethod, url);

        bool responseHeaderExists =
            responseMesssage.Headers.TryGetValues(key, out IEnumerable<string>? values);

        Assert.True(responseHeaderExists);
        Assert.Equal(2, values.Count());
        Assert.True(values.Contains(expectedValueOne) && values.Contains(expectedValueTwo));
    }
}
