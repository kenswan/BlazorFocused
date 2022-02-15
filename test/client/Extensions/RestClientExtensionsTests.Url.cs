using BlazorFocused.Tools.Model;
using Bogus;
using System.Net;
using Xunit;

namespace BlazorFocused.Client.Extensions
{
    public partial class RestClientExtensionsTests
    {
        [Fact]
        public async Task ShouldFormatUrlResponseRequests()
        {
            var relativeUrl = RestClientTestExtensions.GenerateRelativeUrl();
            var requestVariableCount = new Faker().Random.Int(2, 5);
            var requestVariables = GenerateRequestVariables(requestVariableCount);
            var response = RestClientTestExtensions.GenerateResponseObject();

            var expectedUrl = $"{relativeUrl}?" + 
                string.Join("&", requestVariables.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            var httpMethods = new HttpMethod[] {
                HttpMethod.Delete, HttpMethod.Get, HttpMethod.Patch, HttpMethod.Post, HttpMethod.Put };

            void BuilderAction(IRestClientUrlBuilder builder)
            {
                builder = builder.SetPath(relativeUrl);

                foreach (var variableKey in requestVariables.Keys)
                    builder = builder
                        .WithParameter(variableKey, requestVariables[variableKey]);
            }

            foreach (var httpMethod in httpMethods)
                simulatedHttp.GetHttpSetup(httpMethod, expectedUrl, null)
                    .ReturnsAsync(HttpStatusCode.OK, response);

            foreach (var httpMethod in httpMethods)
                _ = await MakeBuilderRequest<SimpleClass>(httpMethod, BuilderAction, null);

            foreach (var httpMethod in httpMethods)
                simulatedHttp.VerifyWasCalled(httpMethod, expectedUrl);
        }

        private Task<T> MakeBuilderRequest<T>(
            HttpMethod httpMethod, Action<IRestClientUrlBuilder> action, object request)
        {
            return httpMethod switch
            {
                HttpMethod method when method == HttpMethod.Delete => restClient.DeleteAsync<T>(action),
                HttpMethod method when method == HttpMethod.Get => restClient.GetAsync<T>(action),
                HttpMethod method when method == HttpMethod.Patch => restClient.PatchAsync<T>(action, request),
                HttpMethod method when method == HttpMethod.Post => restClient.PostAsync<T>(action, request),
                HttpMethod method when method == HttpMethod.Put => restClient.PutAsync<T>(action, request),
                _ => throw new ArgumentException($"{httpMethod} not supported"),
            };
        }

        private static Dictionary<string, string> GenerateRequestVariables(int count)
        {
            var variables = new Dictionary<string, string>();

            for (int i = 0; i < count; i++)
                variables.Add(GenerateString(), GenerateString());

            return variables;
        }

        private static string GenerateString() =>
            new Faker().Random.String2(new Faker().Random.Int(20, 30));
    }
}
