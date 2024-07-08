using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SystemLibrary.Common.Net;

public static class Services
{
    internal static IServiceCollection ServiceCollectionInstance;

    internal static IServiceProvider ServiceProviderInstance;

    public static IServiceCollection Configure()
    {
        ServiceCollectionInstance = new ServiceCollection();

        return ServiceCollectionInstance;
    }

    public static void Configure(IServiceCollection serviceCollection)
    {
        ServiceCollectionInstance = serviceCollection;
    }

    public static void Configure(IServiceProvider serviceProvider)
    {
        ServiceProviderInstance = serviceProvider;
    }

    public static T Get<T>() where T : class
    {
        return ServiceProviderInstance?.GetService<T>();
    }

    public static void AddScoped<T, TImpementation>()
        where T : class
        where TImpementation : class, T
    {
        if (ServiceCollectionInstance == null)
            throw new Exception("You are calling 'AddScoped()' of " + typeof(T).Name + " too early, first call both overload method Services.Configure()");

        ServiceCollectionInstance.AddScoped<T, TImpementation>();
    }

    public static void AddSingleton<T, TImpementation>()
        where T : class
        where TImpementation : class, T
    {
        if (ServiceCollectionInstance == null)
            throw new Exception("You are calling 'AddSingleton()' of " + typeof(T).Name + " too early, first call both overload method Services.Configure()");

        ServiceCollectionInstance.AddSingleton<T, TImpementation>();
    }

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