# BlazorFocused

Adding Flux Architecture and other utilities to Blazor Components

## Installation

Package Manager

```powershell
Install-Package BlazorFocused
```

.NET CLI

```powershell
dotnet add package BlazoFocused
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

    // add store in DI
    builder.Services.AddStore<TestStore>(builder =>
    {
        builder.RegisterAction<TestAction>();
        builder.RegisterAction<TestActionAsync>();
        builder.RegisterReducer<TestReducer, TestStoreSubset>();

        // Services within actions and reducers
        builder.RegisterService<TestService>();
        builder.RegisterHttpClient<ITestClient, TestClient>();

        // Set initial state (optional)
        builder.SetInitialState(new TestStore { FieldOne = "Test" });
    });
}
```

### Server-side Blazor Startup

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    // add store in DI
    serviceCollection.AddStore<TestStore>(builder =>
    {
        builder.RegisterAction<TestAction>();
        builder.RegisterAction<TestActionAsync>();
        builder.RegisterReducer<TestReducer, TestStoreSubset>();

        // Services within actions and reducers
        builder.RegisterService<TestService>();
        builder.RegisterHttpClient<ITestClient, TestClient>();

        // Set initial state (optional)
        builder.SetInitialState(new TestStore { FieldOne = "Test" });
    });
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

store.Reduce<TestStoreSubset>(reducedState =>
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
