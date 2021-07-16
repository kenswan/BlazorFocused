using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Xunit;

namespace BlazorFocused.Client.Test
{
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

            restClient.UpdateHttpClient(client =>
            {
                Assert.True(client.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> actualValues));
                Assert.Single(actualValues);
                Assert.Equal(headerValue, actualValues.FirstOrDefault());
                Assert.Equal(url, client.BaseAddress.OriginalString);
            });
        }
    }
}
