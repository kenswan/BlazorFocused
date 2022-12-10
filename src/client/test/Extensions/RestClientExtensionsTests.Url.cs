// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using Bogus;
using System.Net;
using Xunit;

namespace BlazorFocused.Extensions;

public partial class RestClientExtensionsTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldFormatUrlResponseRequests(HttpMethod httpMethod)
    {
        await SetupExecuteAndVerify(httpMethod, (BuilderAction) =>
            MakeBuilderResponseRequest<SimpleClass>(httpMethod, BuilderAction, null));
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldFormatUrlTaskRequests(HttpMethod httpMethod)
    {
        await SetupExecuteAndVerify(httpMethod, (BuilderAction) =>
            MakeBuilderTaskRequest(httpMethod, BuilderAction, null));
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldFormatUrlTryResponseRequests(HttpMethod httpMethod)
    {
        await SetupExecuteAndVerify(httpMethod, (BuilderAction) =>
            MakeBuilderTryResponseRequest<SimpleClass>(httpMethod, BuilderAction, null));
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldFormatUrlTryTaskRequests(HttpMethod httpMethod)
    {
        await SetupExecuteAndVerify(httpMethod, (BuilderAction) =>
            MakeBuilderTryTaskRequest(httpMethod, BuilderAction, null));
    }

    private async Task SetupExecuteAndVerify(
        HttpMethod httpMethod,
        Func<Action<IRestClientUrlBuilder>, Task> executeMethod)
    {
        var relativeUrl = RestClientTestExtensions.GenerateRelativeUrl();
        var requestVariableCount = new Faker().Random.Int(2, 5);
        Dictionary<string, string> requestVariables = RestClientTestExtensions.GenerateRequestParameters(requestVariableCount);
        SimpleClass response = RestClientTestExtensions.GenerateResponseObject();

        var expectedUrl = $"{relativeUrl}?" +
            string.Join("&", requestVariables.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        void BuilderAction(IRestClientUrlBuilder builder)
        {
            builder = builder.SetPath(relativeUrl);

            foreach (var variableKey in requestVariables.Keys)
            {
                builder = builder
                    .WithParameter(variableKey, requestVariables[variableKey]);
            }
        }

        simulatedHttp.GetHttpSetup(httpMethod, expectedUrl, null)
            .ReturnsAsync(HttpStatusCode.OK, response);

        await executeMethod(BuilderAction);

        simulatedHttp.VerifyWasCalled(httpMethod, expectedUrl);
    }

    private Task<T> MakeBuilderResponseRequest<T>(
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

    private Task MakeBuilderTaskRequest(
        HttpMethod httpMethod, Action<IRestClientUrlBuilder> action, object request)
    {
        return httpMethod switch
        {
            HttpMethod method when method == HttpMethod.Delete => restClient.DeleteTaskAsync(action),
            HttpMethod method when method == HttpMethod.Patch => restClient.PatchTaskAsync(action, request),
            HttpMethod method when method == HttpMethod.Post => restClient.PostTaskAsync(action, request),
            HttpMethod method when method == HttpMethod.Put => restClient.PutTaskAsync(action, request),
            _ => throw new ArgumentException($"{httpMethod} not supported"),
        };
    }

    private Task<RestClientResponse<T>> MakeBuilderTryResponseRequest<T>(
        HttpMethod httpMethod, Action<IRestClientUrlBuilder> action, object request)
    {
        return httpMethod switch
        {
            HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteAsync<T>(action),
            HttpMethod method when method == HttpMethod.Get => restClient.TryGetAsync<T>(action),
            HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchAsync<T>(action, request),
            HttpMethod method when method == HttpMethod.Post => restClient.TryPostAsync<T>(action, request),
            HttpMethod method when method == HttpMethod.Put => restClient.TryPutAsync<T>(action, request),
            _ => throw new ArgumentException($"{httpMethod} not supported"),
        };
    }

    private Task<RestClientTask> MakeBuilderTryTaskRequest(
        HttpMethod httpMethod, Action<IRestClientUrlBuilder> action, object request)
    {
        return httpMethod switch
        {
            HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteTaskAsync(action),
            HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchTaskAsync(action, request),
            HttpMethod method when method == HttpMethod.Post => restClient.TryPostTaskAsync(action, request),
            HttpMethod method when method == HttpMethod.Put => restClient.TryPutTaskAsync(action, request),
            _ => throw new ArgumentException($"{httpMethod} not supported"),
        };
    }
}
