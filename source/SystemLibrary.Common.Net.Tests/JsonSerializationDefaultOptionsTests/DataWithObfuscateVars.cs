namespace SystemLibrary.Common.Net.Tests;

public class DataWithObfuscateVars
{
    public int Id { get; set; }
    public int Id2;
    [JsonObfuscate]
    public int ID3 { get; set; }
    [JsonObfuscate]
    public int id4;
    [JsonObfuscate]
    public int id5;
    [JsonObfuscate]
    public int id6 { get; set; }
    [JsonObfuscate]
    public string id7 { get; set; }
    [JsonObfuscate]
    public string ID8 { get; set; }
    [JsonObfuscate]
    public string Id9 { get; set; }

    public string Hello { get; set; }
    public string World { get; set; }
    public bool b { get; set; }

    [JsonObfuscate]
    public long id11 { get; set; }
}
