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

### Startup

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    // initialized instance of the store's data
    var testClass = new TestClass { FieldOne = "Test" };

    // add store in DI
    serviceCollection.AddStore(testClass, builder =>
    {
        builder.RegisterAction<TestAction>();
        builder.RegisterActionAsync<TestActionAsync>();
        builder.RegisterReducer<TestReducer, TestClassSubset>();

        // Services within actions and reducers
        builder.RegisterService<TestService>();
        builder.RegisterHttpClient<ITestClient, TestClient>();
    });
}
```

### State

Retrieve static state value from store:

```csharp
@inject IStore<TestClass> store;

...

store.GetState().FieldOne;

```

Retrieve state value and subscribe to store updates:

```csharp
@inject IStore<TestClass> store;

...
TestClass currentState = default;

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
@inject IStore<TestClass> store;

...
TestClassSubset currentState = default;

store.Reduce<TestClassSubset>(reducedState =>
{
    currentState = reducedState;

    // inform blazor page to refresh with state update
    StateHasChanged();
});
```

### Actions

Execute actions to update store:

```csharp
@inject IStore<TestClass> store;

...
TestClass currentState = default;

// call action to be committed
store.Dispatch<TestAction>();

store.Subscribe((newState) => {
    // update state used in page
    currentState = newState;

    // inform blazor page to refresh with state update
    StateHasChanged();
});

```
