using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SystemLibrary.Common.Net;

/// <summary>
/// Service Locator
/// </summary>
public static class Services
{
    internal static IServiceCollection ServiceCollectionInstance;

    internal static IServiceProvider ServiceProviderInstance;

    public static IServiceCollection Configure()
    {
        ServiceCollectionInstance = new ServiceCollection();

        return ServiceCollectionInstance;
    }

    /// <summary>
    /// Pass a reference to the Service Collection which will be used to add or remove services from
    /// </summary>
    public static void Configure(IServiceCollection serviceCollection)
    {
        ServiceCollectionInstance = serviceCollection;
    }

    /// <summary>
    /// Pass a reference to the Service Provider which will be used to loook up services from
    /// </summary>
    public static void Configure(IServiceProvider serviceProvider)
    {
        ServiceProviderInstance = serviceProvider;
    }

    /// <summary>
    /// Get service as T or default if not found
    /// </summary>
    public static T Get<T>() where T : class
    {
        return ServiceProviderInstance?.GetService<T>();
    }

    /// <summary>
    /// Add a scoped service to the service collection
    /// </summary>
    public static void AddScoped<T, TImpementation>()
        where T : class
        where TImpementation : class, T
    {
        if (ServiceCollectionInstance == null)
            throw new Exception("You are calling 'AddScoped()' of " + typeof(T).Name + " too early, first call both overload method Services.Configure()");

        ServiceCollectionInstance.AddScoped<T, TImpementation>();
    }

    /// <summary>
    /// Add a service as singleton to the service collection
    /// </summary>
    public static void AddSingleton<T, TImpementation>()
        where T : class
        where TImpementation : class, T
    {
        if (ServiceCollectionInstance == null)
            throw new Exception("You are calling 'AddSingleton()' of " + typeof(T).Name + " too early, first call both overload method Services.Configure()");

        ServiceCollectionInstance.AddSingleton<T, TImpementation>();
    }

    /// <summary>
    /// Remove a service from the service collection
    /// </summary>
    public static void Remove<T>()
    {
        if (ServiceCollectionInstance == null)
            throw new Exception("You are calling 'Remove()' of " + typeof(T).Name + " too early, first call both overload method Services.Configure()");

        var type = typeof(T);
        if (type.IsClass || type.IsInterface)
        {
            ServiceCollectionInstance.RemoveAll<T>();
        }
        else
        {
            throw new Exception(typeof(T).Name + " is not an interface nor a class, cannot be removed");
        }
    }
}