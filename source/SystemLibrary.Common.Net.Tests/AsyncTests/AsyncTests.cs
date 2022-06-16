using System;
using System.Net.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.AsyncTests
{
    [TestClass]
    public class AsyncTests
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

        static void Ex(Exception ex)
        {
            Dump.Write("Error: " + ex);
        }

        [TestMethod]
        public void Test()
        {
            Async.FireAndForget(Ex,() => Call());
            Async.FireAndForget(Ex, () => Call());
            Async.FireAndForget(Ex, () => Call());

            Async.FireAndForget(Ex, () => Call());
            Async.FireAndForget(Ex, () => Call());
            Async.FireAndForget(Ex, () => Call());

            Async.FireAndForget(Ex, () => Call());
            Async.FireAndForget(Ex, () => Call());
            Async.FireAndForget(Ex, () => Call());

            Async.FireAndForget(Ex, () => Call());
            Async.FireAndForget(Ex, () => Call());
            Async.FireAndForget(Ex, () => Call());
            System.Threading.Thread.Sleep(4000);
        }

        static void Call()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://systemlibrary.episerver.demo");
                var response = client.GetStringAsync("")
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}