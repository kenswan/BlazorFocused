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
            SimpleClass responseObject)
        {
            simulatedHttp
                .Setup(httpMethod, relativeRequestUrl)
                .ReturnsAsync(httpStatusCode, responseObject);

            using var client = simulatedHttp.HttpClient;

            await MakeRequest(client, httpMethod, relativeRequestUrl);

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
            SimpleClass requestObject,
            SimpleClass responseObject)
        {
            // simulatedHttp
            //     .Setup(httpMethod, relativeRequestUrl)

            ISimulatedHttpSetup setup = httpMethod switch
            {
                { } when httpMethod == HttpMethod.Delete => simulatedHttp.SetupDELETE(relativeRequestUrl),
                { } when httpMethod == HttpMethod.Get => simulatedHttp.SetupGET(relativeRequestUrl),
                { } when httpMethod == HttpMethod.Patch => simulatedHttp.SetupPATCH(relativeRequestUrl, responseObject),
                { } when httpMethod == HttpMethod.Post => simulatedHttp.SetupPOST(relativeRequestUrl),
                { } when httpMethod == HttpMethod.Put => simulatedHttp.SetupPUT(relativeRequestUrl),
                _ => null
            };
                // .ReturnsAsync(httpStatusCode, responseObject);

            using var client = simulatedHttp.HttpClient;
            await MakeRequest(client, httpMethod, relativeRequestUrl);
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

            Action action = () => simulatedHttp.VerifyWasCalled();

            action.Should().Throw<SimulatedHttpTestException>();
        }
    }
}
