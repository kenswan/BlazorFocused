﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Bogus;
using Xunit;

namespace BlazorFocused.Client;

public class RestClientRequestHeadersTests
{
    private IRestClientRequestHeaders requestHeaders;

    public RestClientRequestHeadersTests()
    {
        requestHeaders = new RestClientRequestHeaders();
    }

    [Fact]
    public void ShouldInitializeDefaultRequestHeaders() => Assert.False(requestHeaders.HasValues());

    [Fact]
    public void ShouldInitializeExistingRequestHeaders()
    {
        var existingHeaders = new Dictionary<string, List<string>>
        {
            { GenerateRandomString() , new List<string> { GenerateRandomString() } }
        };

        requestHeaders = new RestClientRequestHeaders(existingHeaders);

        Assert.True(requestHeaders.HasValues());
    }

    [Fact]
    public void ShouldAddHeaders()
    {
        string header = GenerateRandomString();
        string value = GenerateRandomString();

        requestHeaders.AddHeader(header, value);

        Assert.True(requestHeaders.HasValues());
    }

    [Fact]
    public void ShouldGetHeaderKeys()
    {
        string header = GenerateRandomString();
        string value = GenerateRandomString();

        var existingHeaders = new Dictionary<string, List<string>>
        {
            { header , new List<string> { value } }
        };

        requestHeaders = new RestClientRequestHeaders(existingHeaders);

        IEnumerable<string> keys = requestHeaders.GetHeaderKeys();

        Assert.Single(keys);
        Assert.Equal(header, keys.FirstOrDefault());
    }

    [Fact]
    public void ShouldGetHeaderValues()
    {
        string header = GenerateRandomString();
        string value = GenerateRandomString();

        var existingHeaders = new Dictionary<string, List<string>>
        {
            { header , new List<string> { value } }
        };

        requestHeaders = new RestClientRequestHeaders(existingHeaders);

        string actualValue = requestHeaders.GetHeaderValues(header).FirstOrDefault();

        Assert.Equal(value, actualValue);
    }

    [Fact]
    public void ShouldClearHeaders()
    {
        string header = GenerateRandomString();
        string value = GenerateRandomString();

        var existingHeaders = new Dictionary<string, List<string>>
        {
            { header , new List<string> { value } }
        };

        requestHeaders = new RestClientRequestHeaders(existingHeaders);

        requestHeaders.ClearHeaders();

        Assert.False(requestHeaders.HasValues());
    }

    [Fact]
    public void ShouldAddMultipleHeadersSameKey()
    {

    }

    private static string GenerateRandomString() => new Faker().Random.AlphaNumeric(new Faker().Random.Int(10, 20));
}
