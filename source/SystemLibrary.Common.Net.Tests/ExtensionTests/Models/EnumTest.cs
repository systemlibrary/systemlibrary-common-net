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
        C,

        [EnumValue("dd")]
        [EnumText("Dd")]
        d,

        e,

        _997,

        [EnumValue(value: 998)]
        _999
    }
}
