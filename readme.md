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

## Requirements
- &gt;= .NET 7

## Latest Release Notes
- 7.11.0.1
- Json Encoder reused as a singleton (optimization)
- Proof read somewhat all comments and adjusted accordingly
- Added comments yet again to nuget package so consumers of package can see them through intellisense
- Remarks removed from "comment", and added to the docs output website, hidden from intellisense
- Return comment moved to return section instead of as "last part of comment"
- ToServerMapPath() returns forward slashes paths to support Linux (breaking change)
- ToServerMapPath() renamed to ToAppPath (breaking change)
- Cryptation Env Key "SYSLIBCRYPTATIONKEY" removed (breaking change)
- Cryptation Key and IV either defaults, based on your input, or auto based on 'data key file' (breaking change)
- Services (Service Locator) added, with Configure method for both ServiceCollection and ServiceProvider
- TypeExtension.IsKeyValuePair() added

#### Major breaking changes list
- 7.10 to 7.11
- Cryptation totally redone, Encrypt() now always generates random IV and IV is part of output cipher text
- Cryptation Env. Key "SYSLIBCRYPTATIONKEY" removed
- Services (a service locator) added
 
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