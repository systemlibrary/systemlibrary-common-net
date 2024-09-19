using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net.Configurations;

/// <summary>
/// Enum list of common built-in environment names allowed
/// <para>Config tranformations and EnvironmentName will work with other env-names, but the IsLocal, IsTest, and IsProd will not</para>
/// </summary>
public enum EnvironmentName
{
    [EnumValue("")]
    None,
    Local,
    Dev,
    Development,
    UnitTest,
    QA,
    AT,
    Stage,
    Staging,
    Test,
    PreProd,
    PreProduction,
    Prod,
    Production
}
