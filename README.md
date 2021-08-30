[![Nuget Version](https://img.shields.io/nuget/v/BlazorFocused?logo=nuget)](https://www.nuget.org/packages/BlazorFocused)
[![Nuget Downloads](https://img.shields.io/nuget/dt/BlazorFocused?logo=nuget)](https://www.nuget.org/packages/BlazorFocused)
![Build & Test](https://github.com/kenswan/BlazorFocused/actions/workflows/build.yml/badge.svg)

# BlazorFocused

Providing flux architecture and other tools for Blazor components and .NET development.

## Installation

Install with .NET CLI

```powershell
dotnet add package BlazorFocused
```

OR

Add the following line in your csproj package references

```xml
<PackageReference Include="BlazorFocused" Version="1.x.x" />
```

## Quick Start

### WebAssembly Blazor Startup

```csharp  
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    builder.RootComponents.Add<App>("#app");

    builder.Services.AddScoped(sp =>
        new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

    builder.Services.AddStore<TestStore>(new TestStore { FieldOne = "Initialized" })
        .AddTransient<TestAction>()
        .AddTransient<TestActionAsync>()
        .AddTransient<TestReducer>()
        .AddScoped<TestService>()
        .AddSingleton<TestSingletonService>();

    await builder.Build().RunAsync();
}
```

### Server-side Blazor Startup

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    services.AddStore<TestStore>(new TestStore { FieldOne = "Initialized" })
        .AddTransient<TestAction>()
        .AddTransient<TestActionAsync>()
        .AddTransient<TestReducer>()
        .AddScoped<TestService>()
        .AddSingleton<TestSingletonService>();
}
```

### State

Retrieve static state value from store:

```csharp
@inject IStore<TestStore> store;

...

store.GetState().FieldOne;

```

Retrieve state value and subscribe to store updates:

```csharp
@inject IStore<TestStore> store;

...
TestStore currentState = default;

store.Subscribe((newState) => {
    // update state used in page
    currentState = newState;

    // inform blazor page to refresh with state update
    StateHasChanged();
});

```

### Reducers

Subscribe to reduced value from store:

```csharp
@inject IStore<TestStore> store;

...
TestStoreSubset subsetState = default;

store.Reduce<TestReducer, TestStoreSubset>(reducedState =>
{
    // helpful if you do not care about the full state in your component
    subsetState = reducedState;

    // inform blazor page to refresh with state update
    StateHasChanged();
});
```

### Actions

Execute actions to update store:

```csharp
@inject IStore<TestStore> store;

...
TestStore currentState = default;

// call action to be committed
// if action updates state, component will update
store.Dispatch<TestAction>();

store.Subscribe((newState) => {
    // update state used in page
    currentState = newState;

    // inform blazor page to refresh with state update
    StateHasChanged();
});
```
