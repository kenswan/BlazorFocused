using BlazorFocused.Store;
using Integration.Shared.Models;
using Integration.ToDo.Actions;
using Integration.ToDo.Models;
using Integration.ToDo.Reducers;
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

            builder.Services.AddScoped(sp =>
                new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services
                .AddTransient<AddToDoItem>()
                .AddTransient<GetToDoItems>()
                .AddTransient<ToDoCountReducer>();

            builder.Services.AddStore(
                new User { FirstName = "Default", LastName = "User", UserName = "DefaultUser" });

            builder.Services.AddStore(ToDoStore.GetInitialState());

            await builder.Build().RunAsync();
        }
    }
}
