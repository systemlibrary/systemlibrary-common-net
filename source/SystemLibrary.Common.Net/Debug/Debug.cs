namespace SystemLibrary.Common.Net;

internal static class Debug
{
    static bool? _Debugging;

    static bool Debugging
    {
        get
        {
            if(_Debugging == null)
            {
                _Debugging = AppSettings.Current?.SystemLibraryCommonNet?.Debug == true;
            }
            return _Debugging.Value;
        }
    }

    internal static void Write(string msg)
    {
        if (Debugging)
        {
            Dump.Write("Debug is 'true': " + msg);
        }

    }
}
