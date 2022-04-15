using Bogus;
using Moq;
using Xunit;

namespace BlazorFocused.Client;

public partial class RestClientTests
{
    [Fact]
    public void ShouldUpdateHttpProperties()
    {
        var url = new Faker().Internet.Url();
        var headerKey = "X-PORT-NUMBER";
        var headerValue = new Faker().Internet.Port().ToString();

        restClient.UpdateHttpClient(client =>
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        });

        using var httpClient = (restClient as RestClient).GetClient();

        Assert.True(httpClient.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> actualValues));
        Assert.Single(actualValues);
        Assert.Equal(headerValue, actualValues.FirstOrDefault());
        Assert.Equal(url, httpClient.BaseAddress.OriginalString);
    }

    [Fact]
    public void ShouldAssignRequestHeaders()
    {
        var url = new Faker().Internet.Url();
        var headerKey = "X-PORT-NUMBER";
        var headerValue = new Faker().Internet.Port().ToString();

        restClient.AddHeader(headerKey, headerValue);

        restClientRequestHeadersMock.Verify(headers =>
            headers.AddHeader(headerKey, headerValue), Times.Once());
    }
}
