﻿namespace SystemLibrary.Common.Net.Tests;

public class DataWithCompressedVars
{
    public int Id { get; set; }
    public int Id2;

    [JsonCompress]
    public int ID3 { get; set; }

    [JsonCompress]
    public int id4;

    [JsonCompress]
    public string id7 { get; set; }

    [JsonCompress]
    public string id8;

    [JsonCompress]
    public long id9 { get; set; }
}
