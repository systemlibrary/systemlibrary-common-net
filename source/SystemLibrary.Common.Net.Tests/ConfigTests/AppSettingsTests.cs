namespace SystemLibrary.Common.Net.Tests.Configs;

public class AppSettingsTests : Config<AppSettingsTests>
{
    public string FirstName { get; set; }
    public string lastname { get; set; }
    public bool isEnabled { get; set; }

    public string Username { get; set; }
    public string username { get; set; }
    public string UserName { get; set; }
    public string userName { get; set; }

    public Parent Parent { get; set; }
    public AppSettingsTests()
    {
        Parent = new Parent();
    }
}

public class Parent
{
    public Parent()
    {
        Nested = new Nested();
    }
    public string Color { get; set; }
    public Nested Nested { get; set; }
}

public class Nested
{
    public string Color { get; set; }
    public NestedAgain NestedAgain { get; set; }
    public Nested()
    {
        NestedAgain = new NestedAgain();
    }
}
public class NestedAgain
{
    public string Color { get; set; }
    public Leaf Leaf { get; set; }
    public NestedAgain()
    {
        Leaf = new Leaf();
    }
}

public class Leaf
{
    public string Color { get; set; }
    public int Number { get; set; }
}