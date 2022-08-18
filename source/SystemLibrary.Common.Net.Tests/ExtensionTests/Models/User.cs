using System;

namespace SystemLibrary.Common.Net.Tests.Extensions.DataModel
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
        public DateTime Death { get; set; }
        public DateTimeOffset Married { get; set; }
        public int Age { get; set; }
        public long Money { get; set; }
    }
}
