using System;
using System.Diagnostics;
using System.Text;

using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net;

public abstract class EnvironmentConfig<T, TEnvironmentNameEnum> : Config<T>
    where T : class
    where TEnvironmentNameEnum : struct, IComparable, IFormattable, IConvertible
{
    TEnvironmentNameEnum? _EnvironmentName;
    public TEnvironmentNameEnum EnvironmentName
    {
        get
        {
            if (_EnvironmentName == null)
                return default(TEnvironmentNameEnum);

            return _EnvironmentName.Value;
        }
    }

    internal string GetName() => Name;

    string _Name;
    /// <summary>
    /// Returns environment name based on 'ASPNETCORE_ENVIRONMENT' variable passed to the startup of your application
    /// 
    /// Note: This 'name' is used for transformations for configurations you've created that inherits Config&lt;&gt;
    /// 
    /// Note: Transformations are ran before 'name' here is returned for file 'environmentConfig.json' if it exists
    /// 
    /// Note: changing environment name requires shell restart (iisreset for instance)
    /// </summary>
    /// <example>
    /// Test Explorer
    /// <code class="language-xml hljs">
    /// if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
    ///     then: sets 'temp environment' as value
    ///     
    ///     if: 'temp environment' is set, but no transformation is found
    ///         then: sets 'temp environment' as value from 'Configuration Mode' in Visual Studio
    ///
    /// else:
    ///     then: sets 'temp environment' as value from 'Configuration Mode' in Visual Studio
    /// 
    /// if: environmentConfig.json exists
    ///     if transformation file exists for 'temp environment' 
    ///         then: run transformation for environmentConfig.json
    ///     
    ///     if: environmentConfig.json contains 'name' property
    ///         return: 'value'
    /// 
    /// if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
    ///      return: 'value'
    ///     
    /// if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
    ///     return: 'value'
    /// 
    /// return: "" as 'name', never null
    /// </code>
    /// 
    /// Console Application
    /// <code class="language-xml hljs">
    /// if: environmentConfig.json do not exists:
    ///     if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
    ///         return: 'value'
    ///     
    ///     if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
    ///         return: 'value'
    ///     
    /// else if: 
    ///     if: environmentConfig has transformation equal to 'configuration' pass in as argument
    ///         then: run transformation
    ///         
    ///     if: environmentConfig has property 'name'
    ///         return: 'value'
    /// 
    /// return: "" as 'name', never null
    /// </code>
    /// 
    /// DOTNET TEST 'csproj' --configuration 'release|debug|etc..' command
    /// <code class="language-csharp hljs">
    /// if: environmentConfig.json do not exists:
    ///     if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
    ///         return: 'value'
    ///     
    /// if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
    ///     return: 'value'
    ///     
    /// else if: 
    ///     if: environmentConfig has transformation equal to 'configuration' pass in as argument
    ///         then: run transformation
    ///         
    ///     if: environmentConfig has property 'name'
    ///         return: 'value'
    /// 
    /// return: "" as 'name', never null
    /// </code>
    /// 
    /// IISExpress
    /// <code class="language-xml hljs">
    /// if: launchSettings.json exists
    ///     if: command "IISExpress" exists and contains environment variable 'ASPNETCORE_ENVIRONMENT'
    ///         if: 'ASPNETCORE_ENVIRONMENT' exists
    ///             if: environmentConfig.json exists
    ///                 if: transformation file exists for 'value'
    ///                 then: run transformation
    ///                     
    ///                 if: environmentConfig contains 'name' property  
    ///                 return: 'value'
    ///                 
    ///             return: 'value'
    ///                 
    /// if: 'ASPNETCORE_ENVIRONMENT' exists in web.config
    ///     if: environmentConfig.json exists
    ///         if: transformation file exists for 'value'
    ///         then: run transformation
    ///         
    ///         if: environmentConfig contains 'name' property
    ///             return: 'value'
    ///             
    ///     return: 'value'
    ///         
    /// if: 'ASPNETCORE_ENVIRONMENT' exists as a 'environment variable' in Windows
    ///     if: environmentConfig.json exists
    ///         if: transformation file exists for 'value'
    ///         then: run transformation
    ///         
    ///         if: environmentConfig contains 'name' property
    ///             return: 'value'
    ///             
    ///     return: 'value'
    ///     
    /// if: launchSettings.json exists
    ///     if: "iisSettings" contains "iisExpress" and contains environment variable 'ASPNETCORE_ENVIRONMENT'
    ///         if: environmentConfig.json exists
    ///             if: transformation file exist for 'value'
    ///             then: run transformation
    ///                 
    ///             if: environmentConfig.json contains 'name' property
    ///                 return: 'value'
    ///                         
    ///         return: 'value'
    ///         
    ///     if: "iisSettings" contains environment variable 'ASPNETCORE_ENVIRONMENT'
    ///         if: environmentConfig.json exists
    ///             if: transformation file exists for 'value'
    ///             then: run transformation
    ///                     
    ///             if: environmentConfig.json contains 'name' property
    ///             return: 'value'
    ///                 
    ///         return: 'value'
    ///          
    ///     if: command "IIS" exists and contains environment variable 'ASPNETCORE_ENVIRONMENT'
    ///         if: environmentConfig.json exists
    ///             if: transformation file exists for 'value'
    ///             then: run transformation
    ///                 
    ///             if: environmentConfig.json contains 'name' property
    ///                 return: 'value'
    ///                 
    /// if: environmentConfig.json exists
    ///     if: environmentConfig.json contains 'name' property
    ///         return: 'value'
    ///                         
    /// return: "" as 'name', never null
    /// </code>
    /// 
    /// IIS
    /// <code class="language-xml hljs">
    /// if: 'ASPNETCORE_ENVIRONMENT' exists in web.config
    ///     if: environmentConfig.json exists
    ///         if: transformation file exists 'value' 
    ///         then: run transformation 
    ///         
    ///         if: environmentConfig.json contains 'name' property
    ///             return: 'value'
    ///         
    ///     return: 'value'
    ///     
    /// if: launchSettings.json exists
    ///     if: "iisSettings" contains "iisExpress"
    ///         if: "iisExpress" contains 'environmentVariables'
    ///             if: 'ASPNETCORE_ENVIRONMENT' exists
    ///                 if: environmentConfig.json exists
    ///                     if: transformation file exist for 'value'
    ///                     then: run transformation
    ///                 
    ///                     if: environmentConfig.json contains 'name' property
    ///                         return: 'value'
    ///                     
    ///     if: "iisSettings" contains 'environmentVariables'
    ///         if: 'ASPNETCORE_ENVIRONMENT' exists
    ///             if: environmentConfig.json exists
    ///                 if: transformation file exist for 'value'
    ///                 then: run transformation
    ///                 
    ///                 if: environmentConfig.json contains 'name' property
    ///                     return: 'value'
    ///                     
    ///     if: "profiles" exists
    ///         if: command "IIS" exists and contains 'environmentVariables'
    ///             if: 'ASPNETCORE_ENVIRONMENT' exists
    ///                 if: environmentConfig.json exists
    ///                     if: trnasformation file exists for 'value'
    ///                     then: transformation is ran
    ///                 
    ///                     if: environmentConfig.json contains 'name' property
    ///                         return: 'value'
    ///                         
    ///                 return: 'value'
    ///                 
    /// return: "" as 'name', never null
    /// </code>
    /// </example>
    public string Name
    {
        get
        {
            if (_Name != null && _Name != "")
                return _Name;

            _Name = AspNetCoreEnvironment.Value;
            //TODO: Consider throwing new Exception("Environment 'Name' is not set in either 'ASPNETCORE_ENVIRONMENT', or in environmentConfig.json file, or environmentConfig.json file is located in wrong folder");
            //TODO: Consider this way it works, returns empty name, so it never does any transformations
            //TODO: Consider supporting 'environment' in 'appSettings' for the package: systemLibraryCommonNet { environment { name: '...' } }
            return _Name;
        }
        set
        {
            _Name = value;
            _EnvironmentName = _Name.ToEnum<TEnvironmentNameEnum>();
        }
    }
}

/// <summary>
/// Class containing environment variables read from your 'environmentConfig.json', if you have added one
/// 
/// If you add additional environment configurations to 'environmentConfig.json', then ignore this class, and create your own new class that inherits the generic one: 'EnvironmentConfig&lt;YourClass&gt;'
/// 
/// See documentation for 'EnvironmentConfig&lt;&gt;', specifically the 'Name' property as that is used for all transformations
/// </summary>
public class EnvironmentConfig : EnvironmentConfig<EnvironmentConfig, EnvironmentName>
{
    /// <summary>
    /// Returns true if IsTest and IsProd is false
    /// </summary>
    public static bool IsLocal => !IsProd && !IsTest;

    /// <summary>
    /// Returns true if environment 'name' is 'prod' or 'production', case insensitive
    /// </summary>
    public static bool IsProd => Current.EnvironmentName == EnvironmentName.Prod || Current.EnvironmentName == EnvironmentName.Production;

    /// <summary>
    /// Returns true if environment 'name' is 'Test', 'Stage', 'Staging', 'QA' or 'AT', case insensitive
    /// </summary>
    public static bool IsTest => Current.EnvironmentName == EnvironmentName.Test ||
        Current.EnvironmentName == EnvironmentName.Stage ||
        Current.EnvironmentName == EnvironmentName.Staging ||
        Current.EnvironmentName == EnvironmentName.QA ||
        Current.EnvironmentName == EnvironmentName.AT;
}


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
