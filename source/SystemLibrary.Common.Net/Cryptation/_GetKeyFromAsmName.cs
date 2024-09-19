using System.Reflection;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;

namespace SystemLibrary.Common.Net;

partial class CryptationKey
{
    static string TryGetKeyFromAppNameOrAsmName()
    {
        var keyManagementOptions = Services.Get<IOptions<KeyManagementOptions>>();

        // DataProtection is not added at all; we do not enforce a "Custom Key" based on "AppName"
        if (keyManagementOptions?.Value == null) return null;

        var dataProtectionOptions = Services.Get<IOptions<DataProtectionOptions>>();

        var key = dataProtectionOptions?.Value?.ApplicationDiscriminator;

        // DataProtectionOption was specified, but without key ring and appName, using custom appName as Key
        if (key.IsNot())
        {
            key = GenerateAppName();

            Debug.Log("DataProtection service is added, but without AppName, creating one based on assembly info: " + key.MaxLength(maxLength: 9) + "...");

            dataProtectionOptions.Value.ApplicationDiscriminator = key;
        }

        return key;
    }

    static string GenerateAppName()
    {
        return "AppName" +
               Assembly.GetEntryAssembly()?
               .GetName()?
               .Name?
               .ToLower()?
               .ReplaceAllWith("-", ",", ".", " ", "=", "/", "\\")?
               .MaxLength(32) +
               Assembly.GetCallingAssembly()?
               .GetName()?
               .Name?
               .ToLower()
               .ReplaceAllWith("-", ",", ".", " ", "=", "/", "\\")?
               .MaxLength(4);
    }
}
