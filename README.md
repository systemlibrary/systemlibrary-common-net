# SystemLibrary Common Net

## Description
Library with classes and methods for every &gt;=  .NET 7 application

### Features
- Extensions for Strings, Arrays, Lists, Enums, ... such as Is(), IsNot() and ToEnum()
- Dump.Write() "equivalent" to console.log in javascript
- Config&lt;&gt; class, inherit it and it reads .json config file that has the same name, and also runs transformations based on EnvironmentName
- ToValue and ToText extensions on Enum, through two attributes: EnumText, EnumValue
- Json() on any object, converting to json string or to a C# model from a json string
- JsonPartial() on a json string, to convert only a part of the whole json, into the C# class, no need to model it all
- Fire and forget in Async.Run()!
- Cryptation of string or byte[] through the extension methods: Encrypt() and Decrypt() - uses AES CBC PKCS7
- Obfuscate a string through extension Obfuscate() and Deobfuscate()
- Convert to Base64 for string and byte[] through extension method ToBase64()
- Hash anything through ToSha1(), ToSha256() and ToMd5() extensions for string, byte[] and Stream
- Services.Get() as a service locator

## Requirements
- &gt;= .NET 7

## Latest Release Notes
- 7.18.0.8
- Cryptation key ring debug message now prints correct folder and not the parent of the folder (fix)

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