using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Tools.Client
{
    /// <summary>
    /// <see cref="IHttpClientBuilder"/> extensions that allow Typed Clients to return mock data
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds mock data contained withing <see cref="ISimulatedHttp"/> to Typed Clients
        /// </summary>
        /// <param name="httpClientBuilder">Current <see cref="IHttpClientBuilder"/> in service registration</param>
        /// <param name="simulatedHttp">Provides mock data based on method requests</param>
        /// <returns>Current IHttpClientBuilder with newly registered <see cref="ISimulatedHttp"/></returns>
        public static IHttpClientBuilder AddSimulatedHttp(this IHttpClientBuilder httpClientBuilder, ISimulatedHttp simulatedHttp)
        {
            httpClientBuilder
                .AddHttpMessageHandler<SimulatedVerificationHandler>()
                .AddHttpMessageHandler<SimulatedRequestHandler>()
                .AddHttpMessageHandler<SimulatedResponseHandler>();

            httpClientBuilder.Services.AddSingleton(simulatedHttp);
            httpClientBuilder.Services.AddTransient<SimulatedVerificationHandler>();

            httpClientBuilder.Services.AddTransient(sp =>
            {
                var registeredSimulatedHttp = sp.GetRequiredService<ISimulatedHttp>() as SimulatedHttp;

                return new SimulatedRequestHandler(registeredSimulatedHttp.AddRequest);
            });

            httpClientBuilder.Services.AddTransient(sp =>
            {
                var registeredSimulatedHttp = sp.GetRequiredService<ISimulatedHttp>() as SimulatedHttp;

                return new SimulatedResponseHandler(registeredSimulatedHttp.Responses);
            });

            return httpClientBuilder;
        }
    }
}
