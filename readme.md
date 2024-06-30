# SystemLibrary Common Net

## Description
Library with classes and methods for every &gt;=  .NET 7 application

### Features
- Extensions for Strings, Arrays, Lists, Enums, ... such as Is(), IsNot() and ToEnum()
- Dump.Write() "equivalent" to console.log in javascript
- Config&lt;&gt; class, inherit it and it reads .json config file that has the same name, and also runs transformations based on EnvironmentName
- ToValue and ToText extensions on Enum, through two attributes: EnumText, EnumValue
- ToJson() on any object, converting to json string or to a C# model from a json string
- PartialJson on a json string, to convert only a part of the whole json, into the C# class, no need to model it all
- Fire and forget in Async.Run()!
- Cryptation of string or byte[] through the extension methods: Encrypt() and Decrypt() - uses AES CBC PKCS7
- Obfuscate a string through extension Obfuscate() and Deobfuscate()
- Convert to Base64 for string and byte[] through extension method ToBase64()
- Hash anything through ToSha1(), ToSha256() and ToMd5() extensions for string, byte[] and Stream

## Requirements
- &gt;= .NET 7

## Latest Release Notes
- 7.8.0.1
- StringExtension.IsEmpty function removed, use null check or "is/isnot" instead (breaking change)
- Cryptation.DevelopmentCryptationKey variable removed. One shall use either built-in during dev, or create a proper key
- StringExtension.ReplaceAllWith() now can replace all with a empty string too
- StringExtension.ToDateTimeOffset() added
- JsonConverter for DateTimeOffset uses the ToDateTimeOffset string extension (breaking change)
- ToDateTime stores all culture formats in a static array (optimization)
- StringExtension.EndsWithAnyCaseInsensitive() from ordinal to invariant check
- StringExtension.ToSevrerMapPath reusing static variable from AppData (optimization)
- Dump.Debug moved to root of the configuration for this package (breaking change)
- JsonConverters reusing a static instance for int and long converters (optimization)
 
#### Version history 
- View git history of this file if interested

## Installation
- Simply install the nuget package
- [Installation guide](https://systemlibrary.github.io/systemlibrary-common-net/Install.html)

## Documentation
- [Documentation with code samples](https://systemlibrary.github.io/systemlibrary-common-net/)

## Nuget
- [Nuget package page](https://www.nuget.org/packages/SystemLibrary.Common.Net/)

## Source
- [Github](https://github.com/systemlibrary/systemlibrary-common-net)

## Suggestions and feedback
- [Send us an email](mailto:support@systemlibrary.com)

## License
- Free