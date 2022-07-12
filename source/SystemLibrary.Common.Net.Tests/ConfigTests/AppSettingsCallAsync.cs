using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.ConfigTests;

[TestClass]
public class AppSettingsCallAsync
{
    [TestMethod]
    public void CallAsync_Success()
    {
        var dict = new ConcurrentDictionary<string, string>();

        var tasks = new List<Task>();

        void Add(string value, int i)
        {
            dict.TryAdd(i + "", value);
        }

        void Call(int i)
        {
            Add("Logs", i);
            //NOTE: Replace with line below and mark AppSettings public to test Config-loader in async manner
            //Add(AppSettings.Current.SystemLibraryCommonNet.Dump.Folder, i);
        }

        tasks.Add(Task.Run(() => Call(1)));
        tasks.Add(Task.Run(() => Call(2)));
        tasks.Add(Task.Run(() => Call(3)));
        tasks.Add(Task.Run(() => Call(4)));
        tasks.Add(Task.Run(() => Call(5)));
        tasks.Add(Task.Run(() => Call(6)));
        tasks.Add(Task.Run(() => Call(7)));
        tasks.Add(Task.Run(() => Call(8)));
        tasks.Add(Task.Run(() => Call(9)));
        tasks.Add(Task.Run(() => Call(10)));
        tasks.Add(Task.Run(() => Call(11)));

        var task = Task.WhenAll(tasks.ToArray());

        task.ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        Assert.IsTrue(dict.Count == 11, "Dictionary does not contain 4 responses");

        Assert.IsTrue(dict["3"].Contains("Logs"), "AppSettings was not read properly");
    }
}
