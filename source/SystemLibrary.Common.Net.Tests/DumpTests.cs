using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests
{
    [TestClass]
    public class DumpTests
    {
        [TestMethod]
        public void Dump_Write_And_Clear()
        {
            Dump.Write("Hello world");

            Assert.IsTrue(System.IO.File.Exists("C:\\Logs\\systemlibrary-unit-tests-log.html"));

            System.IO.File.Delete("C:\\Logs\\systemlibrary-unit-tests-log.html");
        }
    }
}
