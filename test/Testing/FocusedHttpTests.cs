using System;
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
            baseAddress = new Faker().Internet.Url();
            focusedHttp = new FocusedHttp(baseAddress);
        }

        [Fact]
        public void ShouldSetBaseUri()
        {
            var client = focusedHttp.Client();

            Assert.Equal(baseAddress, client.BaseAddress.OriginalString);
        }

        [Fact]
        public async Task ShouldReturnSetResponse()
        {
            var requestUrl = "api/test";
            var expectedReponseCode = HttpStatusCode.OK;
            var expectedResponseObject =
                new SimpleClass { FieldOne = "Test1", FieldTwo = "Test2", FieldThree = "Field3" };

            focusedHttp.Setup(request =>
            {
                request.HttpMethod = HttpMethod.Get;
                request.Url = requestUrl;
                return request;
            }, expectedReponseCode, expectedResponseObject);

            var client = focusedHttp.Client();
            var actualResponse = await client.GetAsync(requestUrl);
            var actualStatusCode = actualResponse?.StatusCode;
            var actualResponseString = await actualResponse.Content.ReadAsStringAsync();
            var actualResponseObject = JsonSerializer.Deserialize<SimpleClass>(actualResponseString);

            Assert.Equal(expectedReponseCode, actualResponse.StatusCode);
            actualResponseObject.Should().BeEquivalentTo(expectedResponseObject);
        }

        [Fact]
        public async Task ShouldVerifyRequestMade()
        {
            var requestUrl = "api/test/4";
            var expectedReponseCode = HttpStatusCode.OK;
            var expectedResponseObject =
                new SimpleClass { FieldOne = "Test1", FieldTwo = "Test2", FieldThree = "Field3" };

            focusedHttp.Setup(request =>
            {
                request.HttpMethod = HttpMethod.Get;
                request.Url = requestUrl;
                return request;
            }, expectedReponseCode, expectedResponseObject);

            var client = focusedHttp.Client();
            await client.GetAsync(requestUrl);

            var calledException = Record.Exception(() => focusedHttp.VerifyWasCalled());

            var calledWithMethodException =
                Record.Exception(() => focusedHttp.VerifyWasCalled(HttpMethod.Get));

            var calledWithMethodAndUrlException =
                Record.Exception(() => focusedHttp.VerifyWasCalled(HttpMethod.Get, requestUrl));

            Assert.Null(calledException);
            Assert.Null(calledWithMethodException);
            Assert.Null(calledWithMethodAndUrlException);
        }

        [Fact]
        public void ShouldVerifyRequestNotMade()
        {
            var requestUrl = "api/test/4";
            var expectedReponseCode = HttpStatusCode.OK;
            var expectedResponseObject =
                new SimpleClass { FieldOne = "Test1", FieldTwo = "Test2", FieldThree = "Field3" };

            focusedHttp.Setup(request =>
            {
                request.HttpMethod = HttpMethod.Post;
                request.Url = requestUrl;
                return request;
            }, expectedReponseCode, expectedResponseObject);

            Action act = () => focusedHttp.VerifyWasCalled();
            Action actWithMethod = () => focusedHttp.VerifyWasCalled(HttpMethod.Get);
            Action actWithMethodAndUrl = () => focusedHttp.VerifyWasCalled(HttpMethod.Get, requestUrl);

            act.Should().Throw<FocusedTestException>();

            actWithMethod.Should().Throw<FocusedTestException>()
                .Where(exception => exception.Message.Contains(HttpMethod.Get.ToString()));

            actWithMethodAndUrl.Should().Throw<FocusedTestException>()
                .Where(exception => exception.Message.Contains(HttpMethod.Get.ToString()) &&
                    exception.Message.Contains(requestUrl));
        }
    }
}
