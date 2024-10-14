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

                _Value ??= "";

                Debug.Log("ASPNETCORE_ENVIRONMENT is " + _Value);
            }

            return _Value;
        }
    }
}
