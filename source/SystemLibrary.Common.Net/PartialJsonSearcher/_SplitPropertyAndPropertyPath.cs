using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net
{
    internal static partial class PartialJsonSearcher
    {
        static (string, string[]) SplitPropertyAndPropertyPath<T>(string propertySearchPath)
        {
            string property;
            string[] propertyPaths = null;

            //TODO: update to one stringbuilder/span/...
            if (propertySearchPath?.ContainsAny("/", "\\") == true)
            {
                propertySearchPath = propertySearchPath.TrimEnd("/", "\\");

                var paths = propertySearchPath.Trim().Split(new char[] { '/', '\\' });

                property = paths[^1];

                propertySearchPath = propertySearchPath.Substring(0, propertySearchPath.LastIndexOf(property));
                propertySearchPath = propertySearchPath.TrimEnd("/", "\\");

                propertyPaths = propertySearchPath.Split(new char[] { '/', '\\' });
            }
            else if (propertySearchPath.Is())
            {
                property = propertySearchPath;
            }
            else
            {
                var type = typeof(T);
                property = type.GetTypeName();
                if (type.IsListOrArray())
                    property += "s";
            }

            return (property, propertyPaths);
        }
    }
}
