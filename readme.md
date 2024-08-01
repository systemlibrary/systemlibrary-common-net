# SystemLibrary Common Net

## Description
Library with classes and methods for every &gt;=  .NET 7 application

### Features
- Extensions for Strings, Arrays, Lists, Enums, ... such as Is(), IsNot() and ToEnum()
- Dump.Write() "equivalent" to console.log in javascript
- Config&lt;&gt; class, inherit it and it reads .json config file that has the same name, and also runs transformations based on EnvironmentName
- ToValue and ToText extensions on Enum, through two attributes: EnumText, EnumValue
- Json() on any object, converting to json string or to a C# model from a json string
- PartialJson on a json string, to convert only a part of the whole json, into the C# class, no need to model it all
- Fire and forget in Async.Run()!
- Cryptation of string or byte[] through the extension methods: Encrypt() and Decrypt() - uses AES CBC PKCS7
- Obfuscate a string through extension Obfuscate() and Deobfuscate()
- Convert to Base64 for string and byte[] through extension method ToBase64()
- Hash anything through ToSha1(), ToSha256() and ToMd5() extensions for string, byte[] and Stream
- Services.Get() as a service locator

## Requirements
- &gt;= .NET 7

## Latest Release Notes
- 7.14.0.1
- StringExtensions StartsWithAny, EndsWithAny, ContainsAny now defaults to Comparison Ordinal (breaking change)
- StringExtensions StartsWithAny, EndsWithAny, ContainsAny added overload method that takes StringComparison as first argument
- StringExtensions micro optimizations for checking Array "IsNot()" now checks null and length == 0 internally for string extensions
- IEnumerableExtensions IsNot() micro optimization avoiding a cast to IList in scenarios where count != 1
- StringExtension.EndsWithAnyCaseInsensitive removed (breaking change)
- ToAppPath renamed to ToPhysicalPath (breaking change)

#### Major Breaking Versions
- 7.12.0.1
- Cryptation rewritten, parameterless Encrypt() returns cipher text with a random IV
- Cryptation environment key "SYSLIBCRYPTATIONKEY" removed
- Json() conversions for date time rewritten
- Config files read from 'content root', never /bin/
 
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