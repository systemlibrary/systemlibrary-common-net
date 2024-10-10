using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Extensions.DataModel;
using SystemLibrary.Common.Net.Tests.ExtensionTests;

namespace SystemLibrary.Common.Net.Tests.DumpTests;

[TestClass]
public class DumpTests
{
    const string DumpPath = "C:\\Logs\\systemlibrary-common-net-tests.log";

    [TestMethod]
    public void Dump_Exceptions_With_InnerExceptin_Success()
    {
        var e = new Exception("Hello world1");
        var e2 = new Exception("Outer exception2", e);
        var e3 = new Exception("Furthest out3", e2);
        System.Threading.Thread.Sleep(310);
        Dump.Clear();

        Dump.Write(e3);

        var content = File.ReadAllText(DumpPath);

        Assert.IsTrue(content.Contains("Furthest out3"));
        Assert.IsTrue(content.Contains("Hello world1"));

    }


    [TestMethod]
    public void Dump_List_Poco_WithArrays_Success()
    {
        var owners = new List<Owner>
        {
            new Owner
            {
                Ints = new int[] { 123, 345 },
                Doubles = new double[] { 6666, 7777 },
                Strings = new string[] { "Hello", "World" },
                Name = "AAA"
            },
            new Owner
            {
                Ints = new int[] { 987, 678 },
                Doubles = new double[] { 8888, 1000 },
                Strings = new string[] { "Abcd", "Efgh" },
                Name = "BBB"
            },

        };

        System.Threading.Thread.Sleep(550);

        Dump.Clear();

        Dump.Write(owners);

        var content = File.ReadAllText(DumpPath);
        Assert.IsTrue(content.Contains("World"), "!World");
        Assert.IsTrue(content.Contains("345"), "!345");
        Assert.IsTrue(content.Contains("7777"), "!7777");
        Assert.IsTrue(content.Contains("AAA"), "!AAA");

        Assert.IsTrue(content.Contains("678"), "!678");
        Assert.IsTrue(content.Contains("1000"), "!1000");
        Assert.IsTrue(content.Contains("Efgh"), "!Efgh");
        Assert.IsTrue(content.Contains("BBB"), "!BBB");
    }

    [TestMethod]
    public void Dump_Simple_Write()
    {
        var strings = new string[] { "Hello", "World" };

        Dump.Clear();
        Dump.Write(strings);
    }

    [TestMethod]
    public void Dump_Simple_List_Write()
    {
        var strings = new List<string> { "Hello", "World", "!" };
        Dump.Clear();
        Dump.Write(strings);
    }

    [TestMethod]
    public void Dump_Arrays_Success()
    {
        var strings = new string[] { "Hello", "World" };
        var ints = new int[] { 123, 345 };
        var doubles = new double[] { 6666, 7777 };

        System.Threading.Thread.Sleep(350);

        Dump.Clear();
        Dump.Write(strings);
        Dump.Write(ints);
        Dump.Write(doubles);

        var content = File.ReadAllText(DumpPath);
        Assert.IsTrue(content.Contains("World"), "!World");
        Assert.IsTrue(content.Contains("345"), "!345");
        Assert.IsTrue(content.Contains("7777"), "!7777");
    }

    [TestMethod]
    public void Dump_Write_Exception_Without_StringLng()
    {
        System.Threading.Thread.Sleep(800);
        Exception e = new Exception("Hello world1");

        Dump.Clear();

        Dump.Write(e);
        try
        {
            throw new Exception("Hello world2");
        }
        catch (Exception ex)
        {
            Dump.Write(ex);
        }
        var content = File.ReadAllText(DumpPath);
        Assert.IsTrue(content.Contains("Hello world1"), "!1");
        Assert.IsTrue(content.Contains("Hello world2"), "!2");
        Assert.IsFalse(content.Contains("Length"), "!Length: " + content);
    }

    [TestMethod]
    public void Dump_Write_All_Variable_Types()
    {
        System.Threading.Thread.Sleep(600);
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

        Assert.IsTrue(content.Contains("Hello world"), "!Hello world");
        Assert.IsTrue(content.Contains("multiple lines"), "!multiple lines");
        Assert.IsTrue(content.Contains("True"), "!True");
        Assert.IsTrue(content.Contains("10000"), "!10000");

        Dump.Clear();
    }

    [TestMethod]
    public void Dump_TypeOf_Success()
    {
        System.Threading.Thread.Sleep(500);
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
        System.Threading.Thread.Sleep(700);
        Dump.Write("Hello world");

        Assert.IsTrue(File.Exists(DumpPath));

        Dump.Clear();

        Assert.IsFalse(File.Exists(DumpPath));
    }

    [TestMethod]
    public void Dump_IenumerableString_Success_And_Clear()
    {
        var texts = new List<string>()
        {
            "Hello",
            "World",
            "1234"
        };
        Dump.Clear();

        Dump.Write(texts);
        var content = File.ReadAllText(DumpPath);
        Assert.IsTrue(content.Contains("World"));
        Dump.Clear();

        Dump.Write(texts.ToArray());
        content = File.ReadAllText(DumpPath);
        Assert.IsTrue(content.Contains("World"));
        Dump.Clear();

        var enumerable = texts as IEnumerable<string>;
        Dump.Write(enumerable);
        content = File.ReadAllText(DumpPath);
        Assert.IsTrue(content.Contains("World"));
        Dump.Clear();

        var funcResult = GetEnumerableStringsFromFunction();
        Dump.Write(funcResult);
        content = File.ReadAllText(DumpPath);
        Assert.IsTrue(content.Contains("World"));
        Dump.Clear();
    }

    static IEnumerable<string> GetEnumerableStringsFromFunction()
    {
        yield return "Hello";
        yield return "World";
        yield return "12345";
    }

    [TestMethod]
    public void Dump_Simple_Interface()
    {
        System.Threading.Thread.Sleep(275);
        IOwner iowner = new Owner() { Name = "Interface name..." };

        Dump.Write(iowner);

        var content = File.ReadAllText(DumpPath);

        Assert.IsTrue(content.Contains("Interface name..."));

        Dump.Clear();
    }

    [TestMethod]
    public void Dump_Simple_String()
    {
        System.Threading.Thread.Sleep(300);
        Dump.Write("String");

        var content = File.ReadAllText(DumpPath);

        if (!content.Contains("String"))
            Assert.IsTrue(false, "!String");

        Dump.Clear();
    }

    [TestMethod]
    public void Dump_Simple_Int()
    {
        System.Threading.Thread.Sleep(200);

        Dump.Write(100);

        var content = File.ReadAllText(DumpPath);

        if (!content.Contains("100"))
            Assert.IsTrue(false, "!100");

        Dump.Clear();
    }

    [TestMethod]
    public void Dump_Poco_Success()
    {
        System.Threading.Thread.Sleep(100);

        Dump.Clear();

        var employees = new List<Employee>
        {
            new Employee { Age = 10, TitleFieldName = "DummyTitle", FirstName = "DummyFirstName" }
        };

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
        employees[0].IOwner = new Owner() { Name = "Hello IOwner" };

        employees[0].OwnerPropertyName.ParentEmployee = employees[0];
        employees[0].OwnerPropertyName.ParentEmployee2 = new Employee()
        {
            FirstName = "ParentEmployee2",
            LastNames = new List<string>()
            {
                "Hello","World", "Parent", "Employee", "Another Last Name With Spaces"
            },
            Ages = new int[] { 1, 2, 3, 4, 5 }
        };

        employees[0].OwnerPropertyName.ParentEmployee2.Owner = new User() { LastName = "NOT LOGGED" };
        employees.Add(new Employee { Age = 123123, TitleFieldName = "123123", FirstName = "123123" });
        Dump.Write(employees);

        var content = File.ReadAllText(DumpPath);

        Assert.IsTrue(content.Contains("FieldInt: -8888"), "!FieldInt");
        Assert.IsTrue(content.Contains("OwnerPropertyName"), "!OwnerPropertyName");
        Assert.IsTrue(content.Contains("DummyFirstName"), "!DummyFirstName ");
        Assert.IsTrue(content.Contains("TitleFieldName"), "!TitlePropertyName");
        Assert.IsTrue(content.Contains("IList<Int32> count: 3"), "!FieldListInts");
        Assert.IsTrue(!content.Contains("NOT LOGGED"), "!NOT LOGGED");
        Assert.IsTrue(content.Contains("123123"), "!123123");
        Assert.IsTrue(content.Contains("EnumTestPropertyName"), "!EnumTestPropertyName");
        Assert.IsTrue(content.Contains("99999"), "!99999");
        Assert.IsTrue(content.Contains("IsEnabledNullProperty: True"), "!IsEnabledNullProperty");
        Assert.IsTrue(content.Contains("Married:"), "!Married");

    }

    [TestMethod]
    public void Dump_SkipRuntimeType()
    {
        System.Threading.Thread.Sleep(400);
        Dump.Clear();

        var employees = new List<Employee>();

        var type = employees.GetType();

        Dump.Write(type);

        var content = File.ReadAllText(DumpPath);

        Assert.IsTrue(content?.Length <= 500);

        Dump.Clear();
    }

    [TestMethod]
    public void Dump_Write_Async()
    {
        System.Threading.Thread.Sleep(0);
        var dict = new ConcurrentDictionary<string, string>();

        var tasks = new List<Task>();

        Dump.Clear();

        void Wr(string i)
        {
            Dump.Write(i);
        }

        for (int i = 0; i < 1000; i++)
        {
            tasks.Add(Task.Run(() => Wr(i + " nr")));
        }

        var task = Task.WhenAll(tasks.ToArray());

        task.ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        var content = File.ReadAllText("C:\\Logs\\systemlibrary-common-net-tests.log");

        Assert.IsTrue(content.Length > 10000);

        //Dump.Clear();
    }
}
