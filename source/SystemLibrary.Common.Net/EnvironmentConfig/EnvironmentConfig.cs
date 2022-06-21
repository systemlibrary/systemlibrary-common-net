using SystemLibrary.Common.Net.Attributes;

namespace SystemLibrary.Common.Net
{
    /// <summary>
    /// Class containing various environment specific variables common to all .NET applications based on your 'environmentConfig.json' file
    /// 
    /// You can inherit EnvironmentConfig and implement your own 'IsStaging' for instance
    /// </summary>
    public class EnvironmentConfig : Config<EnvironmentConfig> 
    {
        enum Environment
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
      
        Environment? _EnvironmentName;
        Environment EnvironmentName
        {
            get
            {
                if (_EnvironmentName == null)
                    _EnvironmentName = Name.ToEnum<Environment>();

                return _EnvironmentName.Value;
            }
        }

        string _Name;

        /// <summary>
        /// Returns the environment name based on whats specified in 'ASPNETCORE_ENVIRONMENT', and environmentConfig.json or a combination.
        /// 
        /// The environment name here is used for transformations
        /// 
        /// Note: remember when changing environment variables on windows or requires a restart of the shell (iisreset for instance)
        /// </summary>
        /// <example>
        /// IIS Express:
        /// <code class="language-csharp hljs">
        /// - if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
        ///     return: value as 'name'
        /// - if: ASNETCORE_ENVIRONMENT exists in web.config
        ///     return: value as 'name'
        /// </code>
        ///  
        /// Test Explorer
        /// <code class="language-csharp hljs">
        /// if: Running Tests in 'Test Explorer'
        /// - if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
        ///     then: sets "temp environment" as value
        /// - if: "temp environment" is set, but no transformations are found
        ///     then: sets "temp environment as value from 'Configuration Mode' in Visual Studio
        /// 
        /// - else:
        ///     then: sets "temp environment" as value from 'Configuration Mode' in Visual Studio
        /// 
        /// - if: environmentConfig.json exists
        ///     - if transformation file exists for 'temp environment' 
        ///         then: transformation is ran
        ///     - if: environmentConfig.json contains 'name' property
        ///         return: value as 'name'
        /// 
        /// - if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
        ///     then: value is returned as 'name'
        ///     
        /// - if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
        ///     return: value as 'name'
        /// 
        /// return: "" as 'name', never null
        /// </code>
        /// 
        /// Console Application
        /// <code class="language-csharp hljs">
        /// if: environmentConfig.json do not exists:
        /// - if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
        ///     then: value is returned as 'name' 
        ///     
        /// - if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
        ///     return: value as 'name'
        ///     
        /// else if: 
        ///     if: environmentConfig has transformation equal to 'configuration' pass in as argument
        ///         then: transformation is ran
        ///         
        ///     if: environmentConfig has property 'name'
        ///         return: value as 'name'
        /// 
        /// return: "" as 'name', never null
        /// </code>
        /// 
        /// DOTNET TEST 'csproj' --configuration 'release|debug|etc..' command
        /// <code class="language-csharp hljs">
        /// if: environmentConfig.json do not exists:
        /// - if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
        ///     then: value is returned as 'name' 
        ///     
        /// - if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
        ///     return: value as 'name'
        ///     
        /// else if: 
        ///     if: environmentConfig has transformation equal to 'configuration' pass in as argument
        ///         then: transformation is ran
        ///         
        ///     if: environmentConfig has property 'name'
        ///         return: value as 'name'
        /// 
        /// return: "" as 'name', never null
        /// </code>
        /// 
        /// IIS
        /// <code class="language-csharp hljs">
        /// if: ASPNETCORE_ENVIRONMENT exists in web.config
        ///     return: value as 'name'
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
            }
        }

        bool? _IsLocal;

        /// <summary>
        /// Returns true if IsTest and IsProd is false
        /// </summary>
        public bool IsLocal
        {
            get
            {
                if(_IsLocal == null)
                    _IsLocal = !IsTest && !IsProd;

                return _IsLocal.Value;
            }
        }

        bool? _IsProd;

        /// <summary>
        /// Returns true if environment name is 'prod' or 'production', case insensitive
        /// </summary>
        public bool IsProd
        {
            get
            {
                if (_IsProd == null)
                    _IsProd =
                        EnvironmentName == Environment.Prod ||
                        EnvironmentName == Environment.Production;

                return _IsProd.Value;
            }
        }

        bool? _IsTest;

        /// <summary>
        /// Returns true if environment is 'Test', 'Stage', 'Staging', 'QA' or 'AT', case insensitive
        /// </summary>
        public bool IsTest
        {
            get
            {
                if (_IsTest == null)
                    _IsTest =
                        EnvironmentName == Environment.Test ||
                        EnvironmentName == Environment.Stage ||
                        EnvironmentName == Environment.Staging ||
                        EnvironmentName == Environment.AT ||
                        EnvironmentName == Environment.QA;

                return _IsTest.Value;
            }
        }
    }
}
