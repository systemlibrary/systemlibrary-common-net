using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Tests.Extensions.DataModel;

namespace SystemLibrary.Common.Net.Tests.JsonTokenSearcher
{
    [TestClass]
    public class PartialJsonTest
    {
        static string GetData() => Assemblies.GetEmbeddedResource("_Files", "data.json");

        string TestComplexJsonData = @"{
	""args"": {},
	""data"": """",
	""files"": {},
	""form"": {
		""file"": ""\ufeff{\r\n    \""emptyObject\"": {\r\n        \""empty\"": \r\n    },\r\n    \""somethingAtTheEnd\"": {\r\n        \""end\"": 0\r\n    }\r\n}""
	},
	""headers"": {
		""Content-Length"": ""240"",
		""Content-Type"": ""multipart/form-data; boundary=e9a8eaa0-7e7c-4fb6-bb4d-d891167eb7a4"",
		""Host"": ""httpbin.org"",
		""X-Amzn-Trace-Id"": ""Root=1-62438163-20bba2ec5b462c8633b033be""
	},
	""json"": null,
	""origin"": ""46.212.135.71"",
	""url"": ""http://httpbin.org/post""
}";

        [TestMethod]
        public void Convert_Complex_Partial_Json_To_Simple_Poco()
        {
            var d = TestComplexJsonData;

            var form = d.JsonPartial<Form>();

            Assert.IsTrue(form != null && form.file?.Length > 0 && form.file.Contains("emptyObject"));
        }

        [TestMethod]
        public void Read_Employees_TypeName_Plural()
        {
            var data = GetData();

            var employees = data.JsonPartial<List<Employee>>();

            Assert.IsTrue(employees != null);
            Assert.IsTrue(employees.Count == 2);
            Assert.IsTrue(employees[0].FirstName.Contains("1"));
            Assert.IsTrue(employees[0].FirstName.Contains("Employee"));
            Assert.IsTrue(employees[0].Age == 1);
            Assert.IsTrue(employees[0].FieldLong > 5, "Field long");
            Assert.IsTrue(employees[0].FieldShort > 5, "fied short");
            Assert.IsTrue(employees[0].Birth.Year > DateTime.Now.Year);
        }

        [TestMethod]
        public void Read_Employees_Multiple_Times_In_A_Row()
        {
            var data = GetData();
            var employees1 = data.JsonPartial<List<Employee>>();
            var employees2 = data.JsonPartial<List<Employee>>();

            Assert.IsTrue(employees1.Count == employees2.Count && employees1.Count > 0);
            Assert.IsTrue(employees1[0].FirstName.Contains("1"));
            Assert.IsTrue(employees1[0].Age == 1);
            Assert.IsTrue(employees2[0].FirstName.Contains("1"));
            Assert.IsTrue(employees2[0].Age == 1);
        }

        [TestMethod]
        public void Read_FirstName_Returning_First_Value()
        {
            var data = GetData();

            var firstName = data.JsonPartial<string>("firstName");

            Assert.IsTrue(firstName == "FirstNameEmployee1", firstName);
        }
        [TestMethod]
        public void Read_Age_Returning_First_Value_As_Int()
        {
            var data = GetData();

            var age = data.JsonPartial<int>("age");

            Assert.IsTrue(age == 1, age + "");
        }

        [TestMethod]
        public void Read_Employees_Specific_Property()
        {
            var data = GetData();

            var employees = data.JsonPartial<List<Employee>>("Employees");

            Assert.IsTrue(employees != null);
            Assert.IsTrue(employees.Count == 2);
            Assert.IsTrue(employees[0].FirstName.Contains("FirstNameEmployee1"));
            Assert.IsTrue(employees[0].Age == 1);
        }

        [TestMethod]
        public void Read_Users_TypeName_Plurals_First_Hit()
        {
            var data = GetData();

            var users = data.JsonPartial<List<User>>();

            Assert.IsTrue(users != null);
            Assert.IsTrue(users.Count == 2);
            Assert.IsTrue(users[0].FirstName.Contains("FirstNameUsers1"));
            Assert.IsTrue(users[0].Age == 1);
        }

        [TestMethod]
        public void Read_Users_Inner_Property_Case_Sensitive_Path()
        {
            var data = GetData();

            var users = data.JsonPartial<List<User>>("outerList/list/users");

            Assert.IsTrue(users != null);
            Assert.IsTrue(users.Count == 2);
            Assert.IsTrue(users[0].FirstName.Contains("FirstNameListUsers1"));
            Assert.IsTrue(users[0].Age == 1);
        }

        [TestMethod]
        public void Read_Users_InnerProperty_Case_Insensitive_Path()
        {
            var data = GetData();

            var users = data.JsonPartial<List<User>>("OUTERLIST/LisT/USERs");

            Assert.IsTrue(users != null);
            Assert.IsTrue(users.Count == 2);
            Assert.IsTrue(users[0].FirstName.Contains("FirstNameListUsers1"));
            Assert.IsTrue(users[0].Age == 1);
        }

        [TestMethod]
        public void Read_Complex_Poco_With_Int_Where_Real_Is_Returned_ConvertsToInt32()
        {
            var data = Assemblies.GetEmbeddedResource("_Files", "json-serialization-data-real-as-int.json");

            var users = data.JsonPartial<List<User>>("hits");

            Assert.IsTrue(users != null);

            Assert.IsTrue(users.Count == 3, "" + users.Count);

            Assert.IsTrue(users[2].y == 1234569, "Invalid y");
            Assert.IsTrue(users[2].x == 12345, "Invalid x");
            Assert.IsTrue(users[2].longnumber == Int32.MinValue, "Invalid longnumber");
        }

        [TestMethod]
        public void Read_Complex_Poco_With_Score_As_GetOnlyProperty()
        {
            var data = Assemblies.GetEmbeddedResource("_Files", "json-serialization-data-real-as-int.json");

            var users = data.JsonPartial<List<User>>("hits");

            Assert.IsTrue(users != null);

            Assert.IsTrue(users.Count == 3, "" + users.Count);

            Assert.IsTrue(users[2].score == 0, "Score was read, it does not have a private set?");
        }
    }
}
