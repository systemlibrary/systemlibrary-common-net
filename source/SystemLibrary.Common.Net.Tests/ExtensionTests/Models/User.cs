using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SystemLibrary.Common.Net.Tests.Extensions.DataModel
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime Birth { get; set; }
        public DateTime Death { get; set; }

        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime CurrentDate { get; set; }
        public DateTimeOffset Married { get; set; }
        public int Age { get; set; }
        public long Money { get; set; }

        public User Owner { get; set; }

        public User PreviousOwner { get; set; }

        public string[] Names { get; set; }
        public List<string> LastNames { get; set; }
        public int[] Ages { get; set; }
        public bool IsEnabled { get; set; }
        public TimeSpan Expiration { get; set; }

        public int? NullableAgeProperty { get; set; }
        public DateTime? DateTimeNullProperty { get; set; }
        public DateTimeOffset? DateTimeOffsetNullProperty { get; set; }
        public TimeSpan? TimeSpanNullProperty { get; set; }
        public bool? IsEnabledNullProperty { get; set; }

        public int x { get; set; }
        public int y;
        public int score { get; }
        public int number { get; set; }
        public int longnumber { get; set; }
        public double longnumberdecimals { get; set; }

        public int And { get; set; }

        public EnumTest EnumTestProp { get; set; }
    }
}
