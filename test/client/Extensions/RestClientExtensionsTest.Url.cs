using BlazorFocused.Client.Extensions;
using BlazorFocused.Tools.Model;
using Bogus;
using System.Net;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        public async Task ShouldFormatUrlResponseRequests()
        {
            var relativeUrl = new Faker().Internet.UrlRootedPath();
            var keyOne = GetRandomString();
            var valueOne = GetRandomString();
            var keyTwo = GetRandomString();
            var valueTwo = GetRandomString();
            var response = GetRandomResponseObject();
            var expectedUrl = $"{relativeUrl}?{keyOne}={valueOne}&{keyTwo}={valueTwo}";

            var httpMethods = new HttpMethod[] {
                HttpMethod.Delete, HttpMethod.Get, HttpMethod.Patch, HttpMethod.Post, HttpMethod.Put };

            void BuilderAction(IRestClientUrlBuilder builder)
            {
                builder.SetPath(relativeUrl)
                .WithParameter(keyOne, valueOne)
                .WithParameter(keyTwo, valueTwo);
            }

            foreach (var httpMethod in httpMethods)
                GetHttpSetup(httpMethod, expectedUrl, null)
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

        private static string GetRandomString() =>
            new Faker().Random.String2(new Faker().Random.Int(20, 30));
    }
}
