using System;

namespace SystemLibrary.Common.Net.Extensions
{
    /// <summary>
    /// Extension methods for uri's like GetPrimaryDomain(), ...
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Returns the domain part of the uri or blank, never null:
        /// 
        /// https://www.sub1.sub2.sub3.domain.com => domain.com
        /// </summary>
        /// <returns>Primary domain name or "", never null</returns>
        /// <example>
        /// <code class="language-csharp hljs">
        /// var result = new Uri("https://systemlibrary.com").GetPrimaryDomain();
        /// //result is "systemlibrary.com"
        /// 
        /// var result = new Uri("https://systemlibrary.github.io/systemlibrary-common-net/").GetPrimaryDomain();
        /// //result is "github.io"
        /// </code>
        /// </example>
        public static string GetPrimaryDomain(this Uri uri, string topLevelDomain = ".com")
        {
            if (uri == null)
                return "";

            var host = uri.Host;
            if (host.IsNot())
                return "";

            if (!host.Contains("."))
                return host + topLevelDomain;

            var values = host.Split('.');

            var length = values.Length;
            if (length <= 2)
                return host;

            if (values[length - 1].Length > 3)
                return host;

            return values[length - 2] + "." + values[length - 1];
        }
    }
}
