using BlazorFocused.Client;
using BlazorFocused.Store;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToDo.Client;
using ToDo.Client.Actions;
using ToDo.Client.Reducers;
using ToDo.Client.Services;
using ToDo.Client.Stores;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

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
