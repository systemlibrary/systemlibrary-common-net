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
        /// Returns environment name based on 'ASPNETCORE_ENVIRONMENT' variable passed to the startup of your application
        /// 
        /// Note: This 'name' is used for transformations for configurations you've created that inherits Config&lt;&gt;
        /// 
        /// Note: Transformations are ran before 'name' here is returned for file 'environmentConfig.json' if it exists
        /// 
        /// Note: changing environment name requires shell restart (iisreset for instance)
        /// </summary>
        /// <example>
        /// IIS Express:
        /// <code class="language-csharp hljs">
        /// - if: ASPNETCORE_ENVIRONMENT exists in 'Environment Variables on Windows'
        ///     return: value as 'name'
        /// - if: ASNETCORE_ENVIRONMENT exists in web.config
        ///     return: value as 'name'
        ///     
        /// //Configuration Mode in Visual Studio might have an affect, cannot remember, will test and update docs...
        ///     
        /// return: "" as 'name', never null
        /// </code>
        ///  
        /// Test Explorer
        /// <code class="language-xml hljs">
        /// if: Running Tests in 'Test Explorer'
        /// - if: mstest.runsettings contains 'ASPNETCORE_ENVIRONMENT' variable
        ///     then: sets 'temp environment' as value
        /// - if: 'temp environment' is set, but no transformations are found
        ///     then: sets 'temp environment' as value from 'Configuration Mode' in Visual Studio
        ///
        /// - else:
        ///     then: sets 'temp environment' as value from 'Configuration Mode' in Visual Studio
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
        /// <code class="language-xml hljs">
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
        /// <code class="language-xml hljs">
        /// if: ASPNETCORE_ENVIRONMENT exists in web.config
        ///     return: value as 'name'
        ///     
        /// //Configuration Mode in Visual Studio might have an affect, cannot remember, will test and update docs...
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
        /// Returns true if environment 'name' is 'prod' or 'production', case insensitive
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
        /// Returns true if environment 'name' is 'Test', 'Stage', 'Staging', 'QA' or 'AT', case insensitive
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
