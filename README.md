# BlazoRx

Adding Reactive Programming and Flux Architecture to Blazor Components with Microsoft.Extensions.DependencyInjection

## Development

Integrate store in Startup.cs:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    // initialized instance of the store's data
    var sampleClass = new SampleClass { FieldOne = "Test" };

    // add store in DI
    services.AddStore(sampleClass);
}
```

Retrieve current state of store through in-app DI:

```csharp
@inject IStore<SimpleClass> store;

...

store.GetCurrentState().FieldOne;

```
