using System.Text.Json;

namespace SystemLibrary.Common.Net
{
    internal static partial class PartialJsonSearcher
    {
        public static T Search<T>(string json, string propertySearchPath = null, JsonSerializerOptions options = null) where T : class
        {
            if (json.IsNot()) return default;

            options = Default(options);

            var documentOptions = GetJsonDocumentOptions(options);

            (string property, string[] propertyPaths) = SplitPropertyAndPropertyPath<T>(propertySearchPath);

            var jsonDocument = JsonDocument.Parse(json, documentOptions);

            if (jsonDocument == null) return default;

            JsonElement root = GetRootElement(documentOptions, propertyPaths, jsonDocument);

            var jsonElement = GetJsonElement(root, property, 0, documentOptions.MaxDepth);

            if (!jsonElement.HasValue) return default;

            return JsonSerializer.Deserialize<T>(jsonElement.Value.ToString(), options);
        }
    }
}
