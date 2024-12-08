namespace SystemLibrary.Common.Net.Tests;

public class DataWithEncryptedVars
{
    [JsonEncrypt]
    public int Id { get; set; }
    public int Id2;
    [JsonEncrypt]
    public int ID3 { get; set; }
    [JsonEncrypt]
    public int id4;

    [JsonEncrypt]
    public int id5;
    public int id6 { get; set; }
    public string Hello { get; set; }
    public string World { get; set; }
    public bool b { get; set; }
}
