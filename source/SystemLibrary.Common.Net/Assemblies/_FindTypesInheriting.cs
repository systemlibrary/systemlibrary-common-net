using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemLibrary.Common.Net;

partial class Assemblies
{
    static IEnumerable<Type> FindTypesInheriting(Type classType, Type classWithAttribute = null)
    {
        return WhiteListedAssemblies
            .SelectMany(asm => asm.GetTypes())
            .Where(type => classType.IsAssignableFrom(type) &&
                    (classWithAttribute == null || type.IsDefined(classWithAttribute, false))
        );
    }
}