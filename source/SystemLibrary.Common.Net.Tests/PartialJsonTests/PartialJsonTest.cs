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
        public void Convert_ComplexPartialJson_To_SimplePoco()
        {
            var d = TestComplexJsonData;

            var form = d.PartialJson<Form>();

            Assert.IsTrue(form != null && form.file?.Length > 0 && form.file.Contains("emptyObject"));
        }

            [TestMethod]
        public void Read_Employees_TypeName_Plural()
        {
            var data = GetData();

            var employees = data.PartialJson<List<Employee>>();

            Assert.IsTrue(employees != null);
            Assert.IsTrue(employees.Count == 2);
            Assert.IsTrue(employees[0].FirstName.Contains("1"));
            Assert.IsTrue(employees[0].FirstName.Contains("Employee"));
            Assert.IsTrue(employees[0].Age == 1);
        }

        [TestMethod]
        public void Read_Employees_MultipleTimesInARow()
        {
            var data = GetData();
            var employees1 = data.PartialJson<List<Employee>>();
            var employees2 = data.PartialJson<List<Employee>>();

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

            var firstName = data.PartialJson<string>("firstName");

            Assert.IsTrue(firstName == "FirstNameEmployee1");

        }

        [TestMethod]
        public void Read_Employees_Specific_Property()
        {
            var data = GetData();

            var employees = data.PartialJson<List<Employee>>("Employees");

            Assert.IsTrue(employees != null);
            Assert.IsTrue(employees.Count == 2);
            Assert.IsTrue(employees[0].FirstName.Contains("FirstNameEmployee1"));
            Assert.IsTrue(employees[0].Age == 1);
        }

        [TestMethod]
        public void Read_Users_TypeName_Plurasl_FirstHit()
        {
            var data = GetData();

            var users = data.PartialJson<List<User>>();

            Assert.IsTrue(users != null);
            Assert.IsTrue(users.Count == 2);
            Assert.IsTrue(users[0].FirstName.Contains("FirstNameUsers1"));
            Assert.IsTrue(users[0].Age == 1);
        }

        [TestMethod]
        public void Read_Users_InnerProperty_CaseSensitive_Path()
        {
            var data = GetData();

            var users = data.PartialJson<List<User>>("outerList/list/users");

            Assert.IsTrue(users != null);
            Assert.IsTrue(users.Count == 2);
            Assert.IsTrue(users[0].FirstName.Contains("FirstNameListUsers1"));
            Assert.IsTrue(users[0].Age == 1);
        }

        [TestMethod]
        public void Read_Users_InnerProperty_Case_In_Sensitive_Path()
        {
            var data = GetData();

            var users = data.PartialJson<List<User>>("OUTERLIST/LisT/USERs");

            Assert.IsTrue(users != null);
            Assert.IsTrue(users.Count == 2);
            Assert.IsTrue(users[0].FirstName.Contains("FirstNameListUsers1"));
            Assert.IsTrue(users[0].Age == 1);
        }
    }
}
