using System.Collections.Generic;

namespace SystemLibrary.Common.Net.Tests;


public class Attributes
{
    public long id { get; set; }
    public double deci { get; set; }
    public string number { get; set; }
    public int integer { get; set; }
    public long anotherId { get; set; }
    public string name { get; set; }
    public string nameBlank { get; set; }
}

public class Definition
{
    public string name { get; set; }
    public string type { get; set; }
    public string alias { get; set; }
}

public class FieldAliases
{
    public string name { get; set; }
    public string name2 { get; set; }
    public string name3 { get; set; }
}

public class Inner
{
    public Attributes attributes { get; set; }
}

public class DataWithAllNumberTypesModels
{
    public string defaultName { get; set; }
    public FieldAliases fieldAliases { get; set; }
    public List<Definition> definitions { get; set; }
    public List<Inner> inner { get; set; }
}

