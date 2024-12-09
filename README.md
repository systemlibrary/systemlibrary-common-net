# SystemLibrary Common Net

## Description
Library with classes and methods for every &gt;=  .NET 8 application

### Features
- Extensions for Strings, Arrays, Lists, ... such as Is() and IsNot()
- Dump.Write() "equivalent" to console.log in javascript
- Config&lt;&gt; class, inherit it and it reads .json config file that has the same name as the C# class, also runs transformations based on EnvironmentName
- ToValue and ToText extensions on Enum, through two attributes: [EnumText], [EnumValue]
- Json() on any object, converting to json string or to a C# model from a json string
- JsonPartial() on a json string, to convert only a part of the whole json, into the C# class, no need to model it all
- Fire and forget in Async.Run()!
- Encrypt() and Decrypt() through string and byte[] extensions - uses AES CBC PKCS7
- Obfuscate() and Deobfuscate() through string extension
- ToBase64() and FromBase64() through string and byte[] extensions
- ToHash(), ToSha1(), ToSha256() through Stream, string and byte[] extensions
- Service Locator in Services.Get&lt;&gt;()

## Requirements
- &gt;= .NET 8

## Latest Release Notes
- 8.1.0.1
- Obfuscate micro optimized declaring reusable var outside and removed span (optimization)
- DecryptAttribute renamed to ConfigDecryptAttribute (breaking change)
- JsonEncryptAttribute protects data sent to frontend and decrypts upon receival (new)
- JsonObfuscateAttribute lightly protects data sent to frontend and deobfuscates upon receival (new)

#### Major Breaking Versions
- 7.12.0.1
- Cryptation rewritten, parameterless Encrypt() returns cipher text with a random IV
- Cryptation environment key "SYSLIBCRYPTATIONKEY" removed
- Json() conversions for date time rewritten
- ContentRootPath is never within /bin (exception: .Tests.csproj projects), so Config files are never read from within bin-folder
 
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