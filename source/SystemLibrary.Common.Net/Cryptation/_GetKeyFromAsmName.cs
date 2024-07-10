using System.Reflection;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;

namespace SystemLibrary.Common.Net;

partial class CryptationKey
{
    static string GetKeyFromAsmName()
    {
        var keyManagementOptions = Services.Get<IOptions<KeyManagementOptions>>();

        // DataProtection is not added at all; we do not enforce a "Custom Key" based on "AppName"
        if (keyManagementOptions?.Value == null)
            return null;

        var dataProtectionOptions = Services.Get<IOptions<DataProtectionOptions>>();

        var key = dataProtectionOptions?.Value?.ApplicationDiscriminator;
        
        // DataProtectionOption was specified, but appName was not set, return custom one
        if (key.IsNot())
        {
            if (AppSettings.Current?.SystemLibraryCommonNet?.Debug == true)
                Dump.Write("Debug is 'true': AssemblyName is used as Key, as DataProtection service is added, but without AppName");

            key = Assembly.GetEntryAssembly()?
                .GetName()?
                .Name?
                .ReplaceAllWith("-", ",", ".", " ", "=", "/", "\\")?
                .MaxLength(32)
                + "AppName";
        }

        return key;
    }
}
