using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorFocused.Core.Test.Model;
using Bogus;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Testing.Test
{
    public class FocusedHttpTests
    {
        private readonly FocusedHttp focusedHttp;
        private readonly string baseAddress;

        public FocusedHttpTests()
        {
            baseAddress = GetRandomUrl();
            focusedHttp = new FocusedHttp(baseAddress);
        }

        [Fact]
        public void ShouldSetBaseUri()
        {
            var client = focusedHttp.Client();

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
            focusedHttp.Setup(request =>
            {
                request.HttpMethod = httpMethod;
                request.Url = relativeRequestUrl;
                return request;
            }, httpStatusCode, responseObject);

            var client = focusedHttp.Client();
            HttpResponseMessage actualResponse = await MakeRequest(client, httpMethod, relativeRequestUrl);
            var actualStatusCode = actualResponse?.StatusCode;
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
            focusedHttp.Setup(request =>
            {
                request.HttpMethod = httpMethod;
                request.Url = relativeRequestUrl;
                return request;
            }, httpStatusCode, responseObject);

            await MakeRequest(focusedHttp.Client(), httpMethod, relativeRequestUrl);

            var calledException = Record.Exception(() => focusedHttp.VerifyWasCalled());

            var calledWithMethodException =
                Record.Exception(() => focusedHttp.VerifyWasCalled(httpMethod));

            var calledWithMethodAndUrlException =
                Record.Exception(() => focusedHttp.VerifyWasCalled(httpMethod, relativeRequestUrl));

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
            focusedHttp.Setup(request =>
            {
                request.HttpMethod = httpMethod;
                request.Url = relativeRequestUrl;
                return request;
            }, httpStatusCode, responseObject);

            await MakeRequest(focusedHttp.Client(), httpMethod, relativeRequestUrl);
            var differentHttpMethod = PickDifferentMethod(httpMethod);
            var differentRelativeUrl = GetRandomRelativeUrl();

            Action actWithMethod = () => focusedHttp.VerifyWasCalled(differentHttpMethod);
            Action actWithMethodAndUrl = () => focusedHttp.VerifyWasCalled(httpMethod, differentRelativeUrl);

            actWithMethod.Should().Throw<FocusedTestException>()
                .Where(exception => exception.Message.Contains(differentHttpMethod.ToString()));

            actWithMethodAndUrl.Should().Throw<FocusedTestException>()
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
            focusedHttp.Setup(request =>
            {
                request.HttpMethod = httpMethod;
                request.Url = relativeRequestUrl;
                return request;
            }, httpStatusCode, responseObject);

            Action act = () => focusedHttp.VerifyWasCalled();

            act.Should().Throw<FocusedTestException>();
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
            switch (httpMethod)
            {
                case HttpMethod method when method == HttpMethod.Delete:
                    return client.DeleteAsync(url);
                case HttpMethod method when method == HttpMethod.Get:
                    return client.GetAsync(url);
                case HttpMethod method when method == HttpMethod.Patch:
                    return client.PatchAsync(url, null);
                case HttpMethod method when method == HttpMethod.Post:
                    return client.PostAsync(url, null);
                case HttpMethod method when method == HttpMethod.Put:
                    return client.PutAsync(url, null);
            }

            return default;
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
