using BlazorFocused.Client;
using BlazorFocused.Store;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoList.Client.Actions;
using ToDoList.Client.Reducers;
using ToDoList.Client.Services;
using ToDoList.Client.Stores;

namespace ToDoList.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddStore<ToDoStore>(new())
                .AddTransient<AddToDoAction>()
                .AddTransient<GetAllToDoAction>()
                .AddTransient<CompleteToDoAction>()
                .AddTransient<RestoreToDoAction>()
                .AddTransient<ToDoCountReducer>();

            builder.Services.AddTransient<IToDoService, ToDoService>();

            builder.Services.AddRestClient();

            await builder.Build().RunAsync();
        }
    }
}
