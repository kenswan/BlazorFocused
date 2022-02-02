using BlazorFocused.Model;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Testing
{
    public partial class SimulatedHttpTests
    {
        [Theory]
        [MemberData(nameof(HttpData))]
        public async Task ShouldVerifyRequestMade(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass requestObject,
            SimpleClass responseObject)
        {
            ISimulatedHttpSetup setup = GetHttpSetup(httpMethod, relativeRequestUrl, requestObject);
            setup.ReturnsAsync(httpStatusCode, responseObject);

            using var client = simulatedHttp.HttpClient;

            await MakeRequest(client, httpMethod, relativeRequestUrl, requestObject);

            var calledException = Record.Exception(() => simulatedHttp.VerifyWasCalled());

            var calledWithMethodException =
                Record.Exception(() => simulatedHttp.VerifyWasCalled(httpMethod));

            var calledWithMethodAndUrlException =
                Record.Exception(() => simulatedHttp.VerifyWasCalled(httpMethod, relativeRequestUrl));

            var calledWithMethodUrlContentException = Record.Exception(() =>
                simulatedHttp.VerifyWasCalled(httpMethod, relativeRequestUrl, requestObject));

            Assert.Null(calledException);
            Assert.Null(calledWithMethodException);
            Assert.Null(calledWithMethodAndUrlException);
            Assert.Null(calledWithMethodUrlContentException);
        }

        [Theory]
        [MemberData(nameof(HttpData))]
        public async Task ShouldVerifyNoRequestWasMade(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass requestObject,
            SimpleClass responseObject)
        {
            ISimulatedHttpSetup setup = GetHttpSetup(httpMethod, relativeRequestUrl, requestObject);
            setup.ReturnsAsync(httpStatusCode, responseObject);

            using var client = simulatedHttp.HttpClient;
            await MakeRequest(client, httpMethod, relativeRequestUrl, requestObject);

            var differentHttpMethod = PickDifferentMethod(httpMethod);
            var differentRelativeUrl = GetRandomRelativeUrl();

            Action actWithMethod = () => simulatedHttp.VerifyWasCalled(differentHttpMethod);
            Action actWithMethodAndUrl = () => simulatedHttp.VerifyWasCalled(httpMethod, differentRelativeUrl);

            Action actWithMethodUrlContent = () =>
                simulatedHttp.VerifyWasCalled(httpMethod, relativeRequestUrl, GetRandomSimpleClass());

            actWithMethod.Should().Throw<SimulatedHttpTestException>()
                .Where(exception => exception.Message.Contains(differentHttpMethod.ToString()));

            actWithMethodAndUrl.Should().Throw<SimulatedHttpTestException>()
                .Where(exception => exception.Message.Contains(httpMethod.ToString()) &&
                    exception.Message.Contains(differentRelativeUrl));

            actWithMethodUrlContent.Should().Throw<SimulatedHttpTestException>()
                .Where(exception => exception.Message.Contains("Request Object"));
        }

        [Theory]
        [MemberData(nameof(HttpData))]
        public void ShouldVerifyNoRequestMade(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass requestObject,
            SimpleClass responseObject)
        {
            ISimulatedHttpSetup setup = GetHttpSetup(httpMethod, relativeRequestUrl, requestObject);
            setup.ReturnsAsync(httpStatusCode, responseObject);

            Action action = () => simulatedHttp.VerifyWasCalled();

            action.Should().Throw<SimulatedHttpTestException>();
        }
    }
}
