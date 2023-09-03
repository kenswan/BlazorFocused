// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Bogus;
using Xunit;

namespace BlazorFocused.Client;

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
        string relativeUrl = RestClientTestExtensions.GenerateRelativeUrl();

        restClientUrlBuilder.SetPath(relativeUrl);

        Assert.Equal(relativeUrl, restClientUrlBuilder.Build());
    }

    [Fact]
    public void ShouldAddRequestParameters()
    {
        string parameter = RestClientTestExtensions.GenerateParameter();
        string parameterValue = RestClientTestExtensions.GenerateParameter();

        string expectedUrl = $"?{parameter}={parameterValue}";

        restClientUrlBuilder.WithParameter(parameter, parameterValue);

        Assert.Equal(expectedUrl, restClientUrlBuilder.Build());
    }

    [Fact]
    public void ShouldAddRequestVariables()
    {
        int requestParamCount = new Faker().Random.Int(2, 5);
        Dictionary<string, string> requestParameters = RestClientTestExtensions.GenerateRequestParameters(requestParamCount);
        Tools.Model.SimpleClass response = RestClientTestExtensions.GenerateResponseObject();

        string expectedUrl = $"?" +
            string.Join("&", requestParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        foreach (string paramKey in requestParameters.Keys)
        {
            restClientUrlBuilder.WithParameter(paramKey, requestParameters[paramKey]);
        }

        Assert.Equal(expectedUrl, restClientUrlBuilder.Build());
    }

    [Fact]
    public void ShouldCombinePathAndVariables()
    {
        string relativeUrl = RestClientTestExtensions.GenerateRelativeUrl();
        int requestParamCount = new Faker().Random.Int(2, 5);
        Dictionary<string, string> requestParameters = RestClientTestExtensions.GenerateRequestParameters(requestParamCount);

        string expectedUrl = $"{relativeUrl}?" +
            string.Join("&", requestParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        restClientUrlBuilder.SetPath(relativeUrl);

        foreach (string paramKey in requestParameters.Keys)
        {
            restClientUrlBuilder.WithParameter(paramKey, requestParameters[paramKey]);
        }

        Assert.Equal(expectedUrl, restClientUrlBuilder.Build());
    }
}
