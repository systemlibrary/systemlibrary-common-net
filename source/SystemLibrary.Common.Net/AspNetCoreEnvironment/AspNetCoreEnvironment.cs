namespace SystemLibrary.Common.Net;

internal static class AspNetCoreEnvironment
{
    static string _Value;

    internal static string Value
    {
        get
        {
            if (_Value == null)
            {
                _Value = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                //TODO: Read EnvironmentName-variable used in web apps through "UseEnvironment()" call, somehow...
                _Value ??= "";

                Debug.Log("ASPNETCORE_ENVIRONMENT is " + _Value);
            }

            return _Value;
        }
    }
}
