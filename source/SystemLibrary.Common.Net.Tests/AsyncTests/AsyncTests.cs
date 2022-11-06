using System;
using System.Net.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.AsyncTests
{
    [TestClass]
    public class AsyncTests
    {
        [TestMethod]
        public void Run_One_Fire_And_Forget_Success()
        {
            var file = @"C:\temp\asynctests" + DateTime.Now.ToString("fff") + ".txt";
            Async.FireAndForget(() => System.IO.File.AppendAllText(file, "Hello world"));

            System.Threading.Thread.Sleep(33);

            var text = System.IO.File.ReadAllText(file);
            Assert.IsTrue(text.Contains("Hello world"));

            System.IO.File.Delete(file);
        }

        static int ExceptionCounter = 0;
        static void Ex(Exception ex)
        {
            ExceptionCounter++;
        }

        [TestMethod]
        public void Run_Multiple_Fire_And_Forget_Success()
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

            Assert.IsTrue(ExceptionCounter > 9, "Exception counter was: " + ExceptionCounter);
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
                client.BaseAddress = new Uri("https://www.systemlibrary.com/unknown-url");
                var response = client.GetStringAsync("")
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}