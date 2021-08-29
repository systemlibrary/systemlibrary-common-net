using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net.Tests.Extensions.DataModel
{
    public enum EnumTest
    {
        A,

        [EnumText("HELLO")]
        [EnumValue("hello123")]
        B,

        [EnumText("Hello World")]
        [EnumValue(100)]
        C
    }
}
