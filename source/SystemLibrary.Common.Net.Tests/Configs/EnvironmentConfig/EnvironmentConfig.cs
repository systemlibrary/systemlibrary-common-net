using SystemLibrary.Common.Net.Configurations;

namespace SystemLibrary.Common.Net.Tests.Configs;

public class EnvironmentConfig : EnvironmentConfig<EnvironmentConfig, EnvironmentName>
{
    public string NewPropertyValue { get; set; }

    public static bool IsProd => Current.EnvironmentName == EnvironmentName.Prod || Current.EnvironmentName == EnvironmentName.Production;

    public static bool IsTest => Current.EnvironmentName == EnvironmentName.Test ||
        Current.EnvironmentName == EnvironmentName.Stage ||
        Current.EnvironmentName == EnvironmentName.Staging ||
        Current.EnvironmentName == EnvironmentName.QA ||
        Current.EnvironmentName == EnvironmentName.AT;

    public static bool IsLocal => !IsTest && !IsProd;

}
