using BlazorFocused.Store;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused;

/// <summary>
/// Service Collection extensions that provide store registration
/// in Startup.cs - ConfigureServices 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a new store within the current application
    /// </summary>
    /// <typeparam name="T">State being kept within store</typeparam>
    /// <param name="services">Service Collection being extended</param>
    /// <param name="initialData">Initial value of state within the store</param>
    public static IServiceCollection AddStore<T>(
        this IServiceCollection services, T initialData) where T : class =>
            services.AddScoped<IStore<T>, Store<T>>(serviceProvider =>
                new Store<T>(initialData, serviceProvider)
            );
}
