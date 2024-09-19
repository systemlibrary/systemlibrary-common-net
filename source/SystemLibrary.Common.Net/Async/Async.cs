using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net;

/// <summary>
/// Run methods in a non-blocking manner
/// </summary>
/// <remarks>
/// Might be removed at some point if little or no usage is found
/// </remarks>
public static class Async
{
    /// <summary>
    /// Execute methods in an async manner, appending each single result to a list, and it halts execution till all functions passed as params has completed 
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class Car {
    ///     public string Name { get; set; }
    /// }
    /// 
    /// class CarApi {
    ///     //Simple dummy method that pretends to return a list of cars based on the name from some API
    ///     List&lt;Car&gt; GetByName(string name) {
    ///         //Client exists in nuget package: SystemLibrary.Common.Web
    ///         return Client.Get&lt;List&lt;Car&gt;&gt;("https://systemlibrary.com/cars/q=?" + name);   
    ///     }
    /// }
    /// 
    /// var carApi = new CarApi();
    /// var cars = Async.Run&lt;Car&gt;(
    ///     () => carApi.GetByName("blue"),
    ///     () => carApi.GetByName("red"),
    ///     () => carApi.GetByName("orange")
    /// ); 
    /// 
    /// // Variable 'cars' is filled after all three api requests has completed.
    /// // Assume we got 1 blue, 0 red and 1 orange
    /// // 'cars' now contain a total of 2 objects of type 'Car'
    /// </code>
    /// </example>
    public static List<T> Run<T>(params Func<T>[] functions)
    {
        void Add(T item, List<T> results)
        {
            if (item == null) return;

            results.Add(item);
        }


        var list = new List<T>();

        var tasks = new List<Task>();

        if (functions.Is())
            foreach (var function in functions)
                if (function != null)
                    tasks.Add(Task.Run(() => Add(function(), list)));

        var task = Task.WhenAll(tasks.ToArray());

        task.ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        return list;
    }

    /// <summary>
    /// Execute methods in a async manner, appending the range of results per function to the same list, and it halts execution till all functions passed as params has completed 
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class Car {
    ///     public string Name { get; set; }
    /// }
    /// 
    /// class CarApi {
    ///     //Simple dummy method that pretends to return a list of cars based on the name from some API
    ///     List&lt;Car&gt; GetByName(string name) {
    ///         //Client exists in nuget package: SystemLibrary.Common.Web
    ///         return Client.Get&lt;List&lt;Car&gt;&gt;("https://systemlibrary.com/cars/q=?" + name);   
    ///     }
    /// }
    /// 
    /// var carApi = new CarApi();
    /// var cars = Async.Run&lt;Car&gt;(
    ///     () => carApi.GetByName("blue"),
    ///     () => carApi.GetByName("red"),
    ///     () => carApi.GetByName("orange")
    /// ); 
    /// 
    /// // Variable 'cars' is filled after all three api requests has completed.
    /// // Assume we got 2 blue, 3 red and 4 orange
    /// // 'cars' now contain a total of 9 objects of type 'Car'
    /// </code>
    /// </example>
    public static List<T> Run<T>(params Func<IEnumerable<T>>[] functions)
    {
        void Add(IEnumerable<T> items, List<T> results)
        {
            if (items.IsNot()) return;

            results.AddRange(items);
        }

        var list = new List<T>();

        var tasks = new List<Task>();

        if (functions.Is())
            foreach (var function in functions)
                if (function != null)
                    tasks.Add(Task.Run(() => Add(function(), list)));

        var task = Task.WhenAll(tasks.ToArray());

        task.ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        return list;
    }

    /// <summary>
    /// Run all actions seperately in a non-blocking way
    /// 
    /// <para>Each action passed is ran in a try catch without notifying callee</para>
    /// 
    /// See the overloaded method if you want to ignore exceptions
    /// </summary>
    /// <remarks>
    /// All functions passed to this is ran in an unordered and non-blocking way
    /// <para>All functions passed will run till completion, erroring or till main thread is shut down</para>
    /// </remarks>
    /// <param name="onError">Callback invoked if an exception occured</param>
    /// <param name="actions">Array of methods to invoke in a non-blocking way</param>
    /// <example>
    /// <code class="language-csharp hljs">
    /// Async.FireAndForget((ex) => Log.Error(ex), () => System.IO.File.AppenAllText("C:\temp\text.log", "hello world"));
    /// </code>
    /// </example>
    public static void FireAndForget(Action<Exception> onError, params Action[] actions)
    {
        if (actions.IsNot()) return;

        foreach (var action in actions)
        {
            Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    if (onError != null)
                        onError(ex);
                }
            }
            );
        }
    }

    /// <summary>
    /// Run all actions seperately in a non-blocking way
    /// 
    /// <para>Each action passed is ran in a try catch without notifying callee</para>
    /// 
    /// See the overloaded method to add a callback for logging exceptions
    /// </summary>
    /// <remarks>
    /// All functions passed to this is ran in an unordered and non-blocking way
    /// <para>All functions passed will run till completion, erroring or till main thread is shut down</para>
    /// </remarks>
    /// <param name="actions">Array of methods to invoke in a non-blocking way</param>
    /// <example>
    /// <code class="language-csharp hljs">
    /// Async.FireAndForget(() => System.IO.File.AppenAllText("C:\temp\text.log", "hello world"));
    /// </code>
    /// </example>
    public static void FireAndForget(params Action[] actions)
    {
        FireAndForget(null, actions);
    }
}
