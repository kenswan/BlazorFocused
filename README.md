# BlazoRx

Adding Reactive Programming and Flux Architecture to Blazor Components with Microsoft.Extensions.DependencyInjection

## Register

Integrate basic store in Startup.cs:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    // initialized instance of the store's data
    var testClass = new TestClass { FieldOne = "Test" };

    // add store in DI
    services.AddStore(testClass);
}
```

Integrate store with reducers in Startup.cs:

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
        builder.RegisterReducer(new TestReducer());

        return builder;
    });
}
```

## State

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

## Reducers

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
