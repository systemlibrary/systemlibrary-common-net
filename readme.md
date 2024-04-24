# SystemLibrary Common Net

## Description
Library with classes and methods for every &gt;=  .NET 7 application

### Features
- Extensions fro Strings, Arrays, Lists, ... such as Is(), IsNot() and ToEnum()
- Inherit the Config class which reads .json configuration file into a C# class with transformations based on 'EnvironmentName', at runtime
- Dump.Write() "equivalent" to console.log in javascript
- ToValue and ToText extensions on Enums, supporting two attributes (EnumText, EnumValue) to fill data to an Enum
- ToJson() on any object, converting to json string or to an object from a json string
- PartialJson on a json string, to convert only a part of the whole json, into the C# class, no need to model it all
- Simple fire and forget in Async.Run()
- Simple cryptation of Strings or Bytes[] through extension method Encrypt() and Decrypt() - uses AES CBC PKCS7
- Simply obfuscate a String through extension Obfuscate() and Deobfuscate()
- Simply convert to a Base64 string for Strings and Byte[] through extension method ToBase64()
- Simply hash a String or Byte[] through extension methods ToSha1(), ToSha256() and ToMd5()


## Requirements
- &gt;= .NET 7

## Latest Version
- 7.3.0.6
- Internal namespace change
- Updated deps

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