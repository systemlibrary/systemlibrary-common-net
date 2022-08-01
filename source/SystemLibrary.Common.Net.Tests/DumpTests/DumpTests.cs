using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.DumpTests
{
    [TestClass]
    public class DumpTests
    {
        const string DumpPath = "C:\\Logs\\systemlibrary-common-net-tests.log";

        [TestMethod] 
        public void Dump_StringBuilder_Test()
        {
            var sb = new StringBuilder("Hello world");
            Dump.Clear();
            Dump.Write(sb);
            Dump.Clear();
        }

        [TestMethod]
        public void Dump_Type_Test()
        {
            Dump.Clear();
            Dump.Write(typeof(string));
            Dump.Write(typeof(StringBuilder));
            Dump.Write(typeof(List<int>));
            Dump.Write(typeof(List<>));
            Dump.Write(typeof(int));
            Dump.Write(typeof(Dump));
            Dump.Write(typeof(Config<>));
            Dump.Write(typeof(Async));

            var content = File.ReadAllText(DumpPath);
            Assert.IsTrue(content.Is());
            Assert.IsTrue(content.Contains("System.String"));
            Assert.IsTrue(content.Contains("Dump"));
            Assert.IsTrue(content.Contains("SystemLibrary.Common.Net.Async"));
            Assert.IsTrue(content.Contains("IsClass"));
            Assert.IsTrue(content.Contains("IsValueType"));
        }

        [TestMethod]
        public void Dump_Clear_Multiple_Times_DoesNotThrow()
        {
            Dump.Clear();
            Dump.Clear();
            Dump.Clear();
            Dump.Write("Hello world");
            Dump.Write("Hello world");
            Dump.Clear();
            Dump.Clear();
            Dump.Clear();
            Dump.Write("Hello world");
            Dump.Clear();
            Dump.Clear();
        }

        [TestMethod]
        public void Dump_Write_And_Clear()
        {
            System.Threading.Thread.Sleep(50);
            Dump.Write("Hello world");

            Assert.IsTrue(File.Exists(DumpPath));

            Dump.Clear();

            Assert.IsFalse(File.Exists(DumpPath));
        }

        [TestMethod]
        public void Dump_Simple_String()
        {
            System.Threading.Thread.Sleep(100);
            Dump.Write("String");

            var content = File.ReadAllText(DumpPath);

            if (!content.Contains("String"))
                Assert.IsTrue(false, "String not existing in output log file");

            Dump.Clear();
        }

        [TestMethod]
        public void Dump_Simple_Int()
        {
            System.Threading.Thread.Sleep(150);
            Dump.Write(100);

            var content = File.ReadAllText(DumpPath);

            if (!content.Contains("100"))
                Assert.IsTrue(false, "100 not existing in output log file");

            Dump.Clear();
        }

        [TestMethod]
        public void Dump_Poco()
        {
            System.Threading.Thread.Sleep(200);
            var employees = new List<Employee>();
            employees.Add(new Employee { Age = 10, Title = "DummyTitle", FirstName = "DummyFirstName" });

            Dump.Write(employees);

            var content = File.ReadAllText(DumpPath);
            if (!content.Contains("DummyFirstName"))
                Assert.IsTrue(false, "DummyFirstName not existing in output log file");

            if (!content.Contains("DummyTitle"))
                Assert.IsTrue(false, "DummyTitle not existing in output log file");

            Dump.Clear();
        }

        [TestMethod]
        public void Dump_SkipRuntimeType()
        {
            System.Threading.Thread.Sleep(250);
            Dump.Clear();

            var employees = new List<Employee>();

            var type = employees.GetType();

            Dump.Write(type);

            var content = File.ReadAllText(DumpPath);

            Assert.IsTrue(content?.Length <= 500);

            Dump.Clear();
        }
    }
}
