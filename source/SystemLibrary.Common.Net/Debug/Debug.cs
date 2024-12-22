namespace SystemLibrary.Common.Net;

internal static class Debug
{
    static bool? _Debugging;

    static bool Debugging
    {
        get
        {
            _Debugging ??= AppSettings.Current?.SystemLibraryCommonNet?.Debug == true;
            return _Debugging.Value;
        }
    }

    internal static void Log(string msg)
    {
        if (Debugging)
        {
            Dump.Write("Debug Net 'true': " + msg);
        }
    }

    internal static void Write(object obj)
    {
        if (obj == null)
            System.IO.File.AppendAllText(@"C:\Logs\debug.log", "(null)\n");
        else
            System.IO.File.AppendAllText(@"C:\Logs\debug.log", obj + "\n");
    }
}
