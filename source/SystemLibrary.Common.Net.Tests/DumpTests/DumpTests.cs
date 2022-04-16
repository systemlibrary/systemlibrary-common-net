using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests
{
    [TestClass]
    public class DumpTests
    {
        const string DumpPath = "C:\\Logs\\systemlibrary-unit-tests-log.txt";

        [TestMethod]
        public void Dump_Write_And_Clear()
        {
            Dump.Write("Hello world");

            Assert.IsTrue(File.Exists(DumpPath));

            Dump.Clear();
            Assert.IsFalse(File.Exists(DumpPath));
        }

        [TestMethod]
        public void Dump_Simple_String()
        {
            Dump.Write("String");

            var content = File.ReadAllText(DumpPath);

            if (!content.Contains("String"))
                Assert.IsTrue(false, "String not existing in output log file");

            Dump.Clear();
        }

        [TestMethod]
        public void Dump_Simple_Int()
        {
            Dump.Write(100);

            var content = File.ReadAllText(DumpPath);

            if (!content.Contains("100"))
                Assert.IsTrue(false, "100 not existing in output log file");

            Dump.Clear();
        }

        [TestMethod]
        public void Dump_Poco()
        {
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
    }
}
