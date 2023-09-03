// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Bogus;
using Moq;
using Xunit;

namespace BlazorFocused.Client;

public partial class RestClientTests
{
    [Fact]
    public void ShouldUpdateHttpProperties()
    {
        string url = new Faker().Internet.Url();
        string headerKey = "X-PORT-NUMBER";
        string headerValue = new Faker().Internet.Port().ToString();

        restClient.UpdateHttpClient(client =>
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        });

        using HttpClient httpClient = (restClient as RestClient).GetClient();

        Assert.True(httpClient.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> actualValues));
        Assert.Single(actualValues);
        Assert.Equal(headerValue, actualValues.FirstOrDefault());
        Assert.Equal(url, httpClient.BaseAddress.OriginalString);
    }

    [Fact]
    public void ShouldAssignRequestHeadersGlobally()
    {
        string url = new Faker().Internet.Url();
        string headerKey = "X-PORT-NUMBER";
        string headerValue = new Faker().Internet.Port().ToString();

        restClient.AddHeader(headerKey, headerValue, true);

        restClientRequestHeadersMock.Verify(headers =>
            headers.AddHeader(headerKey, headerValue), Times.Once());
    }

    [Fact]
    public void ShouldAssignRequestHeadersLocally()
    {
        string url = new Faker().Internet.Url();
        string headerKey = "X-PORT-NUMBER";
        string headerValue = new Faker().Internet.Port().ToString();

        restClient.AddHeader(headerKey, headerValue, false);

        using HttpClient httpClient = (restClient as RestClient).GetClient();

        bool valueExists =
            httpClient.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> values);

        Assert.True(valueExists);
        Assert.Single(values);
        Assert.Equal(headerValue, values.FirstOrDefault());

        restClientRequestHeadersMock.Verify(headers =>
            headers.AddHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
