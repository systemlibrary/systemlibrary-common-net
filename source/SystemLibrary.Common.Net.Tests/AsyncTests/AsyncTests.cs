using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.Tests
{
    [TestClass]
    public partial class AsyncTests
    {
        [TestMethod]
        public void FireAndForget_Test()
        {
            var file = @"C:\temp\asynctests" + DateTime.Now.ToString("fff") + ".txt";
            Async.FireAndForget(() => System.IO.File.AppendAllText(file, "Hello world"));

            System.Threading.Thread.Sleep(33);

            var text = System.IO.File.ReadAllText(file);
            Assert.IsTrue(text.Contains("Hello world"));

            System.IO.File.Delete(file);
        }
    }
}
