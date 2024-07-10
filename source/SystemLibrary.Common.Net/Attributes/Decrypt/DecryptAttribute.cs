using System;

namespace SystemLibrary.Common.Net.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DecryptAttribute : Attribute
{
    public string PropertyName;

    public DecryptAttribute(string propertyName = null)
    {
        PropertyName = propertyName;
    }
}
