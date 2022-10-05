using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Extensions.DataModel;
using SystemLibrary.Common.Net.Tests.ExtensionTests;

namespace SystemLibrary.Common.Net.Tests.DumpTests
{
    [TestClass]
    public class DumpTests
    {
        const string DumpPath = "C:\\Logs\\systemlibrary-common-net-tests.log";

        [TestMethod]
        public void Dump_Write_Exception_Without_StringLng()
        {
            Exception e = new Exception("Hello world1");

            Dump.Clear();

            Dump.Write(e);
            try
            {
                throw new Exception("Hello world2");
            }
            catch(Exception ex)
            {
                Dump.Write(ex);
            }
            var content = File.ReadAllText(DumpPath);
            Assert.IsTrue(content.Contains("Hello world1"), "1");
            Assert.IsTrue(content.Contains("Hello world2") ,"2");
            Assert.IsFalse(content.Contains("Length"), "Length");
        }

        [TestMethod]
        public void Dump_Write_All_Variable_Types()
        {
            var sb = new StringBuilder("Hello world");
            Dump.Clear();
            Dump.Write(false);
            Dump.Write(1);
            Dump.Write(10000);
            Dump.Write(true);
            Dump.Write("Hello world");
            Dump.Write("Hello world\n with multiple lines\n\n\n \t\tHehe");
            Dump.Write("Hello world");
            Dump.Write("Hello world");
            Dump.Write(sb);
            Dump.Write(DateTime.Now);
            Dump.Write('A');
            Dump.Write(EnumExtensionsTests.Colors.Red);

            var content = File.ReadAllText(DumpPath);

            Assert.IsTrue(content.Contains("Hello world"), "Hello world is missing");
            Assert.IsTrue(content.Contains("multiple lines"), "multiple lines is missing");
            Assert.IsTrue(content.Contains("True"), "True is missing");
            Assert.IsTrue(content.Contains("10000"), "10000 is missing");

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
            Dump.Clear();
            System.Threading.Thread.Sleep(200);
            var employees = new List<Employee>();
            employees.Add(new Employee { Age = 10, TitleFieldName = "DummyTitle", FirstName = "DummyFirstName" });

            employees[0].FieldListInts = new List<int>() { 9, 99, 90110 };
            employees[0].FieldBool = true;
            employees[0].FieldInt = -8888;
            employees[0].Owner = new User()
            {
                FirstName = "Inner fire",
                LastName = "Inner last name",
                Age = 99999,
                IsEnabled = true,
                IsEnabledNullProperty = true,
                DateTimeNullProperty = DateTime.Now
            };

            employees[0].PreviousOwner = new User()
            {
                FirstName = "Firstname 2",
                LastName = "Last name 2",
                Age = -99999,
                IsEnabled = false,
                IsEnabledNullProperty = true
            };

            employees[0].OwnerPropertyName = new Owner();
            employees[0].OwnerPropertyName.ParentEmployee = employees[0];
            employees[0].OwnerPropertyName.ParentEmployee2 = new Employee()
            {
                FirstName = "ParentEmployee2",
                LastNames = new List<string>()
                {
                    "Hello","World", "Parent", "Employee", "Count5"
                },
                Ages = new int[] { 1, 2, 3, 4, 5 }
            };

            employees[0].OwnerPropertyName.ParentEmployee2.Owner = new User() { LastName = "NOT LOGGED" };
            employees.Add(new Employee { Age = 123123, TitleFieldName = "123123", FirstName = "123123" });
            Dump.Write(employees);

            var content = File.ReadAllText(DumpPath);

            Assert.IsTrue(content.Contains("FieldInt: -8888"), "FieldInt not output");
            Assert.IsTrue(content.Contains("OwnerPropertyName"), "OwnerPropertyName do not exist");
            Assert.IsTrue(content.Contains("DummyFirstName"), "DummyFirstName not existing in output log file");
            Assert.IsTrue(content.Contains("TitleFieldName"), "TitlePropertyName not existing in output log file");
            Assert.IsTrue(content.Contains("IList<Int32> count: 3"), "FieldListInts not outputted");
            Assert.IsTrue(!content.Contains("NOT LOGGED"), "NOT LOGGED is logged, too deep");
            Assert.IsTrue(content.Contains("123123"), "123123, second object is not logged");
            Assert.IsTrue(content.Contains("EnumTestPropertyName"), "EnumTestPropertyName not existing in output log file");
            Assert.IsTrue(content.Contains("99999"), "99999 not existing in output log file");
            Assert.IsTrue(content.Contains("IsEnabledNullProperty: True"), "IsEnabledNullProperty: True not existing in output log file");
            Assert.IsTrue(content.Contains("Married:"), "Married: not existing");

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
