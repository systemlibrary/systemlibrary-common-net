using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net;

/// <summary>
/// Contains methods for running functions in a non-blocking way
///
/// NOTE: Might be removed at a later point in time or rewritten/restructured completely
/// </summary>
public static class Async
{
    /// <summary>
    /// Execute methods in a async manner, appending each single result to a list, and it halts execution till all functions passed as params has completed 
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class Car {
    ///     public string Name { get; set; }
    /// }
    /// 
    /// class CarApi {
    ///     //Simple dummy method that pretends to return a list of cars based on the name from some API
    ///     List&lt;Car&gt; Get(string name) {
    ///         //HttpBaseClient exists in nuget package: SystemLibrary.Common.Web
    ///         return HttpBaseClient.Get&lt;List&lt;Car&gt;&gt;("https://systemlibrary.com/cars/q=?" + name);   
    ///     }
    /// }
    /// 
    /// var cars = Async.Run&lt;Car&gt;(
    ///     () => new CarApi().GetTop1("ferrari"),
    ///     () => new CarApi().GetTop1("volvo"),
    ///     () => new CarApi().GetTop1("tesla")
    /// ); 
    /// 
    /// //Variable 'cars' is filled after all three api requests has completed.
    /// //Assume we got 1 ferrari, 0 volvo and 1 tesla
    /// //'cars' now contain a total of 2 objects of type 'Car'
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
    ///     List&lt;Car&gt; Get(string name) {
    ///         //HttpBaseClient exists in nuget package: SystemLibrary.Common.Web
    ///         return HttpBaseClient.Get&lt;List&lt;Car&gt;&gt;("https://systemlibrary.com/cars/q=?" + name);   
    ///     }
    /// }
    /// 
    /// var cars = Async.Run&lt;Car&gt;(
    ///     () => new CarApi().GetAll("ferrari"),
    ///     () => new CarApi().GetAll("volvo"),
    ///     () => new CarApi().GetAll("tesla")
    /// ); 
    /// 
    /// //Variable 'cars' is filled after all three api requests has completed.
    /// //Assume we got 2 ferraris, 3 volvos and 4 teslas
    /// //'cars' now contain a total of 9 objects of type 'Car'
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
    /// Once FireAndForget() is invoked, your code continues executing, as all actions passed runs in a non-blocking way, in an unordered way.
    /// Actions passed to the method will run till completion or till main thread is shutdown
    /// 
    /// Each action passed is ran in a try-catch without 'ever' crashing the callee.
    /// - Pass in onError callback which executes if any exception is thrown
    /// </summary>
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
    /// Once FireAndForget() is invoked, your code continues executing, as all actions passed runs in a non-blocking way, in an unordered way.
    /// Actions passed to the method will run till completion or till main thread is shutdown
    /// 
    /// Each action passed is ran in a try-catch without 'ever' crashing the callee.
    /// 
    /// See the overloaded method if you want to handle exceptions that might occur running your 'actions'
    /// </summary>
    /// <param name="onError">Pass in function if some action errors, to log or see the error</param>
    /// <param name="actions">Fire and forget actions...</param>
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
