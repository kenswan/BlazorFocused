using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorFocused.Client;
using BlazorFocused.Store;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Integration.ToDo.Actions;
using Integration.ToDo.Reducers;
using Integration.ToDo.Models;
using Integration.Shared.Models;

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

            builder.Services.AddStore(
                new User { FirstName = "Default", LastName = "User", UserName = "DefaultUser" });

            builder.Services.AddStore<ToDoStore>(storeBuilder =>
            {
                storeBuilder.RegisterAction<AddToDoItem>();
                storeBuilder.RegisterAction<GetToDoItems>();
                storeBuilder.RegisterReducer<ToDoCountReducer, ToDoCounter>();
                storeBuilder.RegisterRestClient(builder.HostEnvironment.BaseAddress);
                storeBuilder.SetInitialState(ToDoStore.GetInitialState());
            });

            await builder.Build().RunAsync();
        }
    }
}
