using System.Collections.Generic;

namespace SystemLibrary.Common.Net.Tests;

public class Data : DataInherited
{
    public bool IsSuccess { get; set; }
    public string NorwegianLetters { get; set; }
    public string TextWithUnicodeCodepoints { get; set; }
}

public class DataInherited
{
    public string IntAsStringProperty { get; set; }
    public string StringProperty { get; set; }
    public List<Product> ListOfTextEnums { get; set; }
    public Product CarEnumAsText { get; set; }
    public Product CarEnumAsNumber { get; set; }

    public CarOwner SubClass { get; set; }
}

public class CarOwner
{
    public Product CarEnumAsNumber { get; set; }
    public Product CarEnumAsText { get; set; }
    public int NumberProperty { get; set; }
    public Product CarEnumPropertyAsNull { get; set; }
}

public enum Product
{
    Car1,
    Car2,
    Car3,
    Car4
}
