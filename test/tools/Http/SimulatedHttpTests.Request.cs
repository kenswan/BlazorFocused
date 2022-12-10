// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using FluentAssertions;
using System.Net;
using Xunit;

namespace BlazorFocused.Tools.Http;

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
        GetHttpSetup(httpMethod, relativeRequestUrl, requestObject)
            .ReturnsAsync(httpStatusCode, responseObject);

        using HttpClient client = simulatedHttp.HttpClient;
        var internalSimulatedHttp = simulatedHttp as SimulatedHttp;

        await MakeRequest(client, httpMethod, relativeRequestUrl, requestObject);

        Exception calledException = Record.Exception(() =>
        {
            Action action = GetVerifyActionGroup(httpMethod);
            action.Invoke();
        });

        Exception calledWithUrlException = Record.Exception(() =>
        {
            Action action = GetVerifyActionGroup(httpMethod, relativeRequestUrl);
            action.Invoke();
        });

        Assert.Null(calledException);
        Assert.Null(calledWithUrlException);

        if (!IsMethodWithoutContent(httpMethod))
        {
            Exception calledWithUrlAndContentException = Record.Exception(() =>
            {
                Action action = GetVerifyActionGroup(httpMethod, relativeRequestUrl, requestObject);
                action.Invoke();
            });

            Assert.Null(calledWithUrlAndContentException);
        }
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
        GetHttpSetup(httpMethod, relativeRequestUrl, requestObject)
            .ReturnsAsync(httpStatusCode, responseObject);

        using HttpClient client = simulatedHttp.HttpClient;
        var internalSimulatedHttp = simulatedHttp as SimulatedHttp;
        await MakeRequest(client, httpMethod, relativeRequestUrl, requestObject);

        HttpMethod differentHttpMethod = PickDifferentMethod(httpMethod);
        var differentRelativeUrl = GetRandomRelativeUrl();

        Action actWithMethod = GetVerifyActionGroup(differentHttpMethod);
        Action actWithMethodAndUrl = GetVerifyActionGroup(httpMethod, differentRelativeUrl);

        actWithMethod.Should().Throw<SimulatedHttpTestException>()
            .Where(exception => exception.Message.Contains(differentHttpMethod.ToString()));

        actWithMethodAndUrl.Should().Throw<SimulatedHttpTestException>()
            .Where(exception => exception.Message.Contains(httpMethod.ToString()) &&
                exception.Message.Contains(differentRelativeUrl));

        if (!IsMethodWithoutContent(httpMethod))
        {
            Action actWithMethodUrlContent =
                GetVerifyActionGroup(httpMethod, relativeRequestUrl, GetRandomSimpleClass());

            actWithMethodUrlContent.Should().Throw<SimulatedHttpTestException>()
                .Where(exception => exception.Message.Contains("Request Object"));
        }
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
        GetHttpSetup(httpMethod, relativeRequestUrl, requestObject)
            .ReturnsAsync(httpStatusCode, responseObject);

        var internalSimulatedHttp = simulatedHttp as SimulatedHttp;

        Action action = GetVerifyActionGroup(httpMethod);

        action.Should().Throw<SimulatedHttpTestException>();
    }

    private Action GetVerifyActionGroup(HttpMethod httpMethod, string url = null, object content = null)
    {
        return httpMethod switch
        {
            { } when httpMethod == HttpMethod.Delete && url is null =>
                () => simulatedHttp.VerifyDELETEWasCalled(),
            { } when httpMethod == HttpMethod.Delete && url is not null =>
                () => simulatedHttp.VerifyDELETEWasCalled(url),

            { } when httpMethod == HttpMethod.Get && url is null =>
                () => simulatedHttp.VerifyGETWasCalled(),
            { } when httpMethod == HttpMethod.Get && url is not null =>
                () => simulatedHttp.VerifyGETWasCalled(url),

            { } when httpMethod == HttpMethod.Patch && url is null =>
                () => simulatedHttp.VerifyPATCHWasCalled(),
            { } when httpMethod == HttpMethod.Patch && url is not null && content is null =>
                () => simulatedHttp.VerifyPATCHWasCalled(url),
            { } when httpMethod == HttpMethod.Patch && url is not null && content is not null =>
                () => simulatedHttp.VerifyPATCHWasCalled(url, content),

            { } when httpMethod == HttpMethod.Post && url is null =>
                () => simulatedHttp.VerifyPOSTWasCalled(),
            { } when httpMethod == HttpMethod.Post && url is not null && content is null =>
                () => simulatedHttp.VerifyPOSTWasCalled(url),
            { } when httpMethod == HttpMethod.Post && url is not null && content is not null =>
                () => simulatedHttp.VerifyPOSTWasCalled(url, content),

            { } when httpMethod == HttpMethod.Put && url is null =>
                () => simulatedHttp.VerifyPUTWasCalled(),
            { } when httpMethod == HttpMethod.Put && url is not null && content is null =>
                () => simulatedHttp.VerifyPUTWasCalled(url),
            { } when httpMethod == HttpMethod.Put && url is not null && content is not null =>
                () => simulatedHttp.VerifyPUTWasCalled(url, content),

            _ => throw new NotImplementedException("Verify Action Group Not Implemented")
        };
    }

    private static bool IsMethodWithoutContent(HttpMethod httpMethod)
    {
        return httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get;
    }
}
