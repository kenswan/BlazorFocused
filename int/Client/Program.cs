using BlazorFocused.Client;
using BlazorFocused.Store;
using Integration.Client.Actions;
using Integration.Client.Models;
using Integration.Client.Reducers;
using Integration.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integration.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services
                .AddTransient<AddToDoItem>()
                .AddTransient<GetToDoItems>()
                .AddTransient<ToDoCountReducer>()
                .AddStore(ToDoStore.GetInitialState());

            // builder.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationProvider>();

            builder.Services.AddScoped<UserAuthenticationProvider>();

            builder.Services.AddScoped<AuthenticationStateProvider>(provider => 
                provider.GetRequiredService<UserAuthenticationProvider>());

            builder.Services.AddRestClient();
            builder.Services.AddOAuthRestClient();

            await builder.Build().RunAsync();
        }
    }
}
