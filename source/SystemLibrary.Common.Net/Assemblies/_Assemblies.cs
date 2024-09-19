using System;
using System.Collections.Generic;

using Asm = System.Reflection.Assembly;

namespace SystemLibrary.Common.Net;

public static partial class Assemblies
{
    static Assemblies()
    {
        var assembliesLoaded = AppDomain.CurrentDomain.GetAssemblies();

        var whiteListedAssemblies = new List<Asm>();
        foreach (var asm in assembliesLoaded)
        {
            if (!asm.FullName.StartsWithAny(BlacklistedAssemblyNames))
                whiteListedAssemblies.Add(asm);
        }
        WhiteListedAssemblies = whiteListedAssemblies;
    }
}