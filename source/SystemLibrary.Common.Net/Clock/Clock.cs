//using System;
//using System.Diagnostics;

//namespace SystemLibrary.Common.Net;

//public static class Clock
//{
//    /// <summary>
//    /// Measure an action and optionally write a dump message with a title
//    /// </summary>
//    /// <example>
//    /// <code>
//    /// var (ms, ticks) = Measure(() => {
//    ///  return "Hello world form an API";
//    /// }, "Hello world");
//    /// // ms and ticks contains time used to return the text
//    /// // a dump message has been written to disc
//    /// </code>
//    /// </example>
//    /// <returns>Time elapsed to run the method, in milliseconds and cpu ticks</returns>
//    public static (long ms, long ticks) Measure(Action method, string title = null)
//    {
//        var sw = new Stopwatch();

//        sw.Start();
//        sw.Stop();
//        sw.Reset();
//        // Executing start stop reset before actual starting, "warming up":

//        sw.Start();

//        method();

//        sw.Stop();

//        if (title != null)
//            Dump.Write(title + " executed in: " + sw.ElapsedTicks + " ticks, " + sw.ElapsedMilliseconds + "ms.");

//        var ms = sw.ElapsedMilliseconds;
//        var ticks = sw.ElapsedTicks;

//        sw.Reset();

//        return (ms, ticks);
//    }
//}
