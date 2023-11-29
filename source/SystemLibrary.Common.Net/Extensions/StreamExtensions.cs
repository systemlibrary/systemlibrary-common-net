using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Net.Extensions;

/// <summary>
/// Extensions for streams like reading content as json directly into class through ToJsonAsync()
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Read a stream as of 'json data' into a class T
    /// 
    /// Usually used whenever receiving content from an API or similar over the network, and with the stream automatically convert its content in an async manner to your class
    /// 
    /// Note: if you do not send in 'JsonSerializerOptions' the common ones from SystemLibrary will be used, which contain a 'String To Enum', 'Int to String' and some other additional converters than the standard that comes with System.Text.json
    /// </summary>
    /// <example>
    /// <code>
    /// using (var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
    ///    return await contentStream.ToJsonAsync&lt;T&gt;(jsonSerializerOptions).ConfigureAwait(false);
    /// </code>
    /// </example>
    public static async Task<T> ToJsonAsync<T>(this Stream stream, JsonSerializerOptions options = null) where T : class
    {
        if (stream == null) return default;

        options = GetJsonSerializerOptions.Default(options);

        return await JsonSerializer.DeserializeAsync<T>(stream, options).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the stream to its end, hashing the content and returning the hash as a string
    /// 
    /// Returns null if stream is null or cannot be read
    /// 
    /// Tip: If data is larger than ~200 bytes then .ToSha1Hash() is faster
    /// </summary>
    /// <example>
    /// <code>
    /// var fileStream = new FileStream(@"C:\file.txt", FileMode.Open);
    /// var hash = fileStream.ToMD5HashAsync().Result;
    /// </code>
    /// </example>
    public static async Task<string> ToMD5HashAsync(this Stream stream)
    {
        return await Md5.ComputeAsync(stream).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the stream to its end, hashing the content and returning the hash as a string
    /// 
    /// Returns null if stream is null or cannot be read
    /// 
    /// Tip: If data is larger than ~200 bytes then .ToSha1Hash() is faster
    /// </summary>
    /// <example>
    /// <code>
    /// var fileStream = new FileStream(@"C:\file.txt", FileMode.Open);
    /// var hash = fileStream.ToMD5Hash();
    /// </code>
    /// </example>
    public static string ToMD5Hash(this Stream stream)
    {
        return Md5.Compute(stream);
    }

    /// <summary>
    /// Reads the stream to its end, hashing the content and returning the hash as a string
    /// 
    /// Returns null if stream is null or cannot be read
    /// 
    /// Tip: If data is smaller than ~200 bytes then .ToMD5Hash() is faster
    /// </summary>
    /// <example>
    /// <code>
    /// var fileStream = new FileStream(@"C:\file.txt", FileMode.Open);
    /// var hash = fileStream.ToSha1Hash();
    /// </code>
    /// </example>
    public static string ToSha1Hash(this Stream stream)
    {
        return Sha1.Compute(stream);
    }

    /// <summary>
    /// Reads the stream to its end, hashing the content and returning the hash as a string
    /// 
    /// Returns null if stream is null or cannot be read
    /// 
    /// Tip: If data is smaller than ~200 bytes then .ToMD5Hash() is faster
    /// </summary>
    /// <example>
    /// <code>
    /// var fileStream = new FileStream(@"C:\file.txt", FileMode.Open);
    /// var hash = fileStream.ToSha1HashAsync().Result;
    /// </code>
    /// </example>
    public static async Task<string> ToSha1HashAsync(this Stream stream)
    {
        return await Sha1.ComputeAsync(stream).ConfigureAwait(false);
    }
}
