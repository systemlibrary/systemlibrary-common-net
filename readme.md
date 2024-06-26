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
- 7.9.0.1
- Environment.ContentRootPath added as singleton (new)
- StringExtensions.ToServerMapPath optimized and uses Environment.ContentRootPath (breaking change)
- Config&lt;T&gt; optimized greatly and uses Environment.ContentRootPath to define root to search in, no longer searches inside Bin (breaking change)
- Internal structure changes and renamings
 
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