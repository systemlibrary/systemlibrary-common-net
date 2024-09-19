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
        public IOwner IOwner { get; set; }
        public long FieldLong { get; set; }
        public short FieldShort { get; set; }
    }

    public interface IOwner
    {
        string Name { get; set; }
    }

    public class Owner : IOwner
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public EnumTest EnumTest { get; set; }

        public Employee ParentEmployee { get; set; }
        public Employee ParentEmployee2 { get; set; }
        public string[] Strings { get; set; }
        public int[] Ints { get; set; }
        public double[] Doubles { get; set; }
    }
}
