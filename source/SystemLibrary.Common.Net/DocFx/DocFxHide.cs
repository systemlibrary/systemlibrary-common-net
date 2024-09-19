using System;

namespace SystemLibrary.Common.Net;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Constructor)]
public class DocFxHide : Attribute
{
    public DocFxHide()
    {
    }
}
