// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlazorFocused;

/// <summary>
/// Extensions used to initialize <see cref="IRestClient"/> and inherited members
/// </summary>
public static class ClientServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures <see cref="IRestClient"/> within service collection
    /// </summary>
    /// <param name="services">Current service collection</param>
    /// <param name="baseUrl">Url for base address of inner http client </param>
    /// <returns>Current service collection with <see cref="IRestClient"/></returns>
    public static IHttpClientBuilder AddRestClient(this IServiceCollection services, string baseUrl)
    {
        return services.AddRestClient(client => client.BaseAddress = new Uri(baseUrl));
    }

    /// <summary>
    /// Adds and configures <see cref="IRestClient"/> within service collection 
    /// </summary>
    /// <param name="services">Current service collection</param>
    /// <param name="configureClient">Inner HttpClient configurations</param>
    /// <returns>Current service collection with <see cref="IRestClient"/></returns>
    public static IHttpClientBuilder AddRestClient(
        this IServiceCollection services,
        Action<HttpClient> configureClient = null)
    {
        services.AddRestClientOptions<RestClient>(registerDefault: true);

        return ((configureClient is null) ?
            services.AddHttpClient<IRestClient, RestClient>() :
            services.AddHttpClient<IRestClient, RestClient>(configureClient))
                .AddHttpMessageHandler<RestClientHeaderHandler>();
    }

    /// <summary>
    /// Adds and configures <see cref="IOAuthRestClient"/> within service collection 
    /// </summary>
    /// <param name="services">Current service collection</param>
    /// <param name="baseUrl">Url for base address of inner http client </param>
    /// <returns>Current service collection with <see cref="IOAuthRestClient"/></returns>
    public static IHttpClientBuilder AddOAuthRestClient(this IServiceCollection services, string baseUrl)
    {
        return services.AddOAuthRestClient(client => client.BaseAddress = new Uri(baseUrl));
    }

    /// <summary>
    /// Adds and configures <see cref="IOAuthRestClient"/> within service collection
    /// </summary>
    /// <param name="services">Current service collection</param>
    /// <param name="configureClient">Inner HttpClient configurations</param>
    /// <returns>Current service collection with <see cref="IOAuthRestClient"/></returns>
    public static IHttpClientBuilder AddOAuthRestClient(
        this IServiceCollection services,
        Action<HttpClient> configureClient = null)
    {
        services.AddRestClientOptions<OAuthRestClient>();
        services.TryAddSingleton<IOAuthToken>(sp => new OAuthToken());
        services.TryAddTransient<RestClientAuthHandler>();

        return ((configureClient is null) ?
            services.AddHttpClient<IOAuthRestClient, OAuthRestClient>()
                .AddHttpMessageHandler<RestClientAuthHandler>() :
            services.AddHttpClient<IOAuthRestClient, OAuthRestClient>(configureClient)
                .AddHttpMessageHandler<RestClientAuthHandler>())
                    .AddHttpMessageHandler<RestClientHeaderHandler>();
    }

    private static void AddRestClientOptions<T>(
        this IServiceCollection services, bool registerDefault = false)
        where T : IRestClient
    {
        var configKey = typeof(T).Name;
        var defaultKey = nameof(RestClient);
        var namedOption = registerDefault == true ? "" : typeof(T).Name;

        services.TryAddSingleton<IRestClientRequestHeaders>(_ => new RestClientRequestHeaders());
        services.TryAddTransient<RestClientHeaderHandler>();

        services
            .AddOptions<RestClientOptions>(namedOption)
            .Configure<IConfiguration>((options, configuration) =>
                (configuration.GetSection(configKey) switch
                {
                    IConfigurationSection section when section.Exists() => section,
                    _ => configuration.GetSection(defaultKey)
                }).Bind(options))
            .Validate(options =>
                string.IsNullOrWhiteSpace(options.BaseAddress) ||
                    Uri.TryCreate(options.BaseAddress, UriKind.Absolute, out _),
                "BaseAddress in configuration is not a valid Uri");
    }
}
