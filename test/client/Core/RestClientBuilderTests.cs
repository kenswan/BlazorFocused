using Bogus;
using Xunit;

namespace BlazorFocused.Client
{
    public class RestClientBuilderTests
    {
        private readonly IRestClientUrlBuilder restClientUrlBuilder;

        public RestClientBuilderTests()
        {
            restClientUrlBuilder = new RestClientUrlBuilder();
        }

        [Fact]
        public void ShouldSetPath()
        {
            var relativeUrl = RestClientTestExtensions.GenerateRelativeUrl();

            restClientUrlBuilder.SetPath(relativeUrl);

            Assert.Equal(relativeUrl, restClientUrlBuilder.Build());
        }

        [Fact]
        public void ShouldAddRequestParameters()
        {
            var parameter = RestClientTestExtensions.GenerateParameter();
            var parameterValue = RestClientTestExtensions.GenerateParameter();

            var expectedUrl = $"?{parameter}={parameterValue}";

            restClientUrlBuilder.WithParameter(parameter, parameterValue);

            Assert.Equal(expectedUrl, restClientUrlBuilder.Build());
        }

        [Fact]
        public void ShouldAddRequestVariables()
        {
            var requestParamCount = new Faker().Random.Int(2, 5);
            var requestParameters = RestClientTestExtensions.GenerateRequestParameters(requestParamCount);
            var response = RestClientTestExtensions.GenerateResponseObject();

            var expectedUrl = $"?" +
                string.Join("&", requestParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            foreach (var paramKey in requestParameters.Keys)
                restClientUrlBuilder.WithParameter(paramKey, requestParameters[paramKey]);

            Assert.Equal(expectedUrl, restClientUrlBuilder.Build());
        }

        [Fact]
        public void ShouldCombinePathAndVariables()
        {
            var relativeUrl = RestClientTestExtensions.GenerateRelativeUrl();
            var requestParamCount = new Faker().Random.Int(2, 5);
            var requestParameters = RestClientTestExtensions.GenerateRequestParameters(requestParamCount);

            var expectedUrl = $"{relativeUrl}?" +
                string.Join("&", requestParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            restClientUrlBuilder.SetPath(relativeUrl);

            foreach (var paramKey in requestParameters.Keys)
                restClientUrlBuilder.WithParameter(paramKey, requestParameters[paramKey]);

            Assert.Equal(expectedUrl, restClientUrlBuilder.Build());
        }
    }
}
