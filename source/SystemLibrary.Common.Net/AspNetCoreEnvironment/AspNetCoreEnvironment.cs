﻿using System;

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
                if (_Value == null)
                    _Value = "";

                if (AppSettings.Current?.SystemLibraryCommonNet?.Debug == true)
                {
                    Dump.Write("Debug is 'true': ASPNETCORE_ENVIRONMENT is " + _Value);
                }
            }

            return _Value;
        }
    }
}
