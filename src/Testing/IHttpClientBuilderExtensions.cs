using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Testing
{
    public static class IHttpClientBuilderExtensions
    {
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
