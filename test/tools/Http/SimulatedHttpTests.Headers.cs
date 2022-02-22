using BlazorFocused.Tools.Utility;
using Bogus;
using System.Net;
using Xunit;

namespace BlazorFocused.Tools.Http
{
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

            await testService.MakeRequestAsync(httpMethod, url);

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

            await testService.MakeRequestAsync(httpMethod, url);

            var actualValue = simulatedHttp.GetRequestHeaderValues(httpMethod, url, key).FirstOrDefault();

            Assert.Equal(expectedValue, actualValue);
        }
    }
}
