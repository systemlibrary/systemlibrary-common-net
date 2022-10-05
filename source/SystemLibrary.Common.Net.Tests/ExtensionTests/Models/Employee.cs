using System.Collections.Generic;

namespace SystemLibrary.Common.Net.Tests.Extensions.DataModel
{
    public class Employee : User
    {
        public string TitleFieldName;
        public int FieldInt;
        public bool FieldBool;
        public List<int> FieldListInts;
        public int SalaryPropertyName { get; set; }
        public EnumTest EnumTestPropertyName { get; set; }
        public Owner OwnerPropertyName { get; set; }
    }

    public class Owner
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public EnumTest EnumTest { get; set; }

        public Employee ParentEmployee { get; set; }
        public Employee ParentEmployee2 { get; set; }
    }
}
