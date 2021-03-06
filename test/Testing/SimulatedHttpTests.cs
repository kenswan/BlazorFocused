using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorFocused.Test.Model;
using Bogus;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Testing.Test
{
    public class SimulatedHttpTests
    {
        private readonly ISimulatedHttp simulatedHttp;
        private readonly string baseAddress;

        public SimulatedHttpTests()
        {
            baseAddress = GetRandomUrl();
            simulatedHttp = new SimulatedHttp(baseAddress);
        }

        [Fact]
        public void ShouldSetBaseUri()
        {
            var client = simulatedHttp.Client();

            Assert.Equal(baseAddress, client.BaseAddress.OriginalString);
        }

        [Theory]
        [MemberData(nameof(HttpData))]
        public async Task ShouldReturnRequestedResponse(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass responseObject)
        {
            simulatedHttp
                .Setup(httpMethod, relativeRequestUrl)
                .ReturnsAsync(httpStatusCode, responseObject);

            var client = simulatedHttp.Client();
            HttpResponseMessage actualResponse = await MakeRequest(client, httpMethod, relativeRequestUrl);
            var actualResponseString = await actualResponse.Content.ReadAsStringAsync();
            var actualResponseObject = JsonSerializer.Deserialize<SimpleClass>(actualResponseString);

            Assert.Equal(httpStatusCode, actualResponse.StatusCode);
            actualResponseObject.Should().BeEquivalentTo(responseObject);
        }

        [Theory]
        [MemberData(nameof(HttpData))]
        public async Task ShouldVerifyRequestMade(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass responseObject)
        {
            simulatedHttp
                .Setup(httpMethod, relativeRequestUrl)
                .ReturnsAsync(httpStatusCode, responseObject);

            await MakeRequest(simulatedHttp.Client(), httpMethod, relativeRequestUrl);

            var calledException = Record.Exception(() => simulatedHttp.VerifyWasCalled());

            var calledWithMethodException =
                Record.Exception(() => simulatedHttp.VerifyWasCalled(httpMethod));

            var calledWithMethodAndUrlException =
                Record.Exception(() => simulatedHttp.VerifyWasCalled(httpMethod, relativeRequestUrl));

            Assert.Null(calledException);
            Assert.Null(calledWithMethodException);
            Assert.Null(calledWithMethodAndUrlException);
        }

        [Theory]
        [MemberData(nameof(HttpData))]
        public async Task ShouldVerifyNoRequestWasMade(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass responseObject)
        {
            simulatedHttp
                .Setup(httpMethod, relativeRequestUrl)
                .ReturnsAsync(httpStatusCode, responseObject);

            await MakeRequest(simulatedHttp.Client(), httpMethod, relativeRequestUrl);
            var differentHttpMethod = PickDifferentMethod(httpMethod);
            var differentRelativeUrl = GetRandomRelativeUrl();

            Action actWithMethod = () => simulatedHttp.VerifyWasCalled(differentHttpMethod);
            Action actWithMethodAndUrl = () => simulatedHttp.VerifyWasCalled(httpMethod, differentRelativeUrl);

            actWithMethod.Should().Throw<SimulatedHttpTestException>()
                .Where(exception => exception.Message.Contains(differentHttpMethod.ToString()));

            actWithMethodAndUrl.Should().Throw<SimulatedHttpTestException>()
                .Where(exception => exception.Message.Contains(httpMethod.ToString()) &&
                    exception.Message.Contains(differentRelativeUrl));
        }

        [Theory]
        [MemberData(nameof(HttpData))]
        public void ShouldVerifyNoRequestMade(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass responseObject)
        {
            simulatedHttp
                .Setup(httpMethod, relativeRequestUrl)
                .ReturnsAsync(httpStatusCode, responseObject);

            Action act = () => simulatedHttp.VerifyWasCalled();

            act.Should().Throw<SimulatedHttpTestException>();
        }

        public static TheoryData<HttpMethod, HttpStatusCode, string, SimpleClass> HttpData =>
            new TheoryData<HttpMethod, HttpStatusCode, string, SimpleClass>
            {
                { HttpMethod.Delete, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Get, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Patch, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Post, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Put, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() }
            };

        private static Task<HttpResponseMessage> MakeRequest(HttpClient client, HttpMethod httpMethod, string url)
        {
            return httpMethod switch
            {
                HttpMethod method when method == HttpMethod.Delete => client.DeleteAsync(url),
                HttpMethod method when method == HttpMethod.Get => client.GetAsync(url),
                HttpMethod method when method == HttpMethod.Patch => client.PatchAsync(url, null),
                HttpMethod method when method == HttpMethod.Post => client.PostAsync(url, null),
                HttpMethod method when method == HttpMethod.Put => client.PutAsync(url, null),
                _ => throw new SimulatedHttpTestException($"{httpMethod} not supported"),
            };
        }

        private static HttpMethod PickDifferentMethod(HttpMethod httpMethod)
        {
            var methods = new List<HttpMethod>
            {
                HttpMethod.Delete,
                HttpMethod.Get,
                HttpMethod.Patch,
                HttpMethod.Post,
                HttpMethod.Put
            };

            methods.Remove(httpMethod);

            return new Faker().PickRandom(methods);
        }

        private static HttpStatusCode GetRandomStatusCode() =>
            new Faker().PickRandom<HttpStatusCode>();

        private static string GetRandomUrl() =>
            new Faker().Internet.Url();

        private static string GetRandomRelativeUrl() =>
            new Faker().Internet.UrlRootedPath();

        private static SimpleClass GetRandomSimpleClass() =>
            new Faker<SimpleClass>()
                .RuleForType(typeof(string), fake => fake.Lorem.Sentence(GetRandomNumber()))
                .Generate();

        private static int GetRandomNumber() =>
            new Faker().Random.Int(4, 10);
    }
}
