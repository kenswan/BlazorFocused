using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorFocused.Client;
using BlazorFocused.Store;
using BlazorFocused.Integration.Client.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorFocused.Integration.Client.Actions;
using BlazorFocused.Integration.Client.Reducers;

namespace BlazorFocused.Integration.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp =>
                new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddStore<ToDoStore>(storeBuilder =>
            {
                storeBuilder.RegisterAsyncAction<GetToDoItems>();
                storeBuilder.RegisterReducer<ToDoCountReducer, ToDoCounter>();
                storeBuilder.RegisterRestClient(builder.HostEnvironment.BaseAddress);
            });

            await builder.Build().RunAsync();
        }
    }
}
