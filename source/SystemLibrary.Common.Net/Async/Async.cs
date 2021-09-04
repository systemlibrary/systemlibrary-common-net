using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Simple way of running multiple non-blocking functions async, which all returns the same T object
    /// 
    /// NOTE: Might be removed at a later point in time or rewritten/restructured completely
    /// </summary>
    /// <example>
    /// <code class="language-csharp hljs">
    /// class Car {
    ///     public string Name { get; set; }
    /// }
    /// 
    /// class CarApi {
    ///     //Simple dummy method that pretends to return a list of cars based on the name
    ///     List&lt;Car&gt; Get(string name) {
    ///         //HttpBaseClient exists in nuget package: System.Library.Web.HttpBaseClient
    ///         return HttpBaseClient.Get&lt;List&lt;Car&gt;&gt;("https://cars.com/q=?" + name);   
    ///     }
    /// }
    /// 
    /// var cars = Async.Run&lt;Car&gt;(
    ///     () => new CarApi().Get("ferrari"),
    ///     () => new CarApi().Get("volvo"),
    ///     () => new CarApi().Get("tesla")
    /// ); 
    /// 
    /// //cars is filled after all 3 api requests has completed, as in returned some result.
    /// //all results are added to the same list object and then returns...
    /// //so 'cars' is a list that now contains ferrari's, volvo's and tesla's
    /// </code>
    /// </example>
    public static class Async
    {
        /// <summary>
        /// Execute methods in a async manner, appending each single result to a list
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var cars = Async.Run&lt;List&lt;Car&gt;&gt;(
        ///     () => new CarService().GetTop1("ferrari"),  //Assume a CarService that has a Top1 method, returns only 1 car max
        ///     () => new CarService().GetTop1("volvo"),
        ///     () => new CarService().GetTop1("tesla")
        /// ); 
        /// //cars now contains 1 ferrari, 1 volvo, 1 tesla - assuming all requests returned an object
        /// //all methods are ran async, as in "parallel" in a non-blocking thread way
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
        /// Execute methods in a async manner, appending the range of results per function to the same list
        /// </summary>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var cars = Async.Run&lt;Car&gt;(
        ///     () => new CarService().Get("ferrari"),
        ///     () => new CarService().Get("volvo"),
        ///     () => new CarService().Get("tesla")
        /// ); 
        /// //cars now contains multiple ferrari's, volvo's and tesla's
        /// //all methods are ran async, as in "parallel" in a non-blocking thread way
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
        /// Run all actions seperately, on different threads.
        /// 
        /// Once the action has started your program continues, and those actions keeps running till completed or the main thread is being shutdown (exit/restart of program)
        /// </summary>
        /// <param name="onError">Pass in function if some action errors, to log or see the error</param>
        /// <param name="actions">Fire and forget actions...</param>
        /// <example>
        /// <code>
        /// Async.FireAndForget(() => System.IO.File.AppenAllText("C:\temp\text.log", "hello world"));;
        /// </code>
        /// </example>
        public static void FireAndForget(Action<string> onError, params Action[] actions)
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
                            onError(ex + "");
                    }
                }
                );
            }
        }

        public static void FireAndForget(params Action[] actions)
        {
            FireAndForget(null, actions);
        }
    }
}
