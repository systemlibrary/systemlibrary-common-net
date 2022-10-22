# SystemLibrary Common Net

## Description
Library with classes and methods for every .NET &gt;= 6 application

### Features
- Extensions for strings, arrays, lists, ... such as "Is()" and "IsNot()"
- Configuration class Config&lt;&gt; which reads your .json config files into C# classes, including transformations
- Dump.Write() "equivalent" to console.log in javascript
- Convert to and from json through .ToJson() on any object
- Convert parts of a json string to a C# class through .PartialJson()
- Simple fire and forget in 'Async.Run()'

## Requirements
- &gt;= .NET 6

## Latest Version
- 6.4.1.5
- Added ~50 assembly names to the 'blacklist', so that this library will not search through those DLLs when using the class 'Assemblies.' in this library (perf boost)
- Added comments after benchmarks for Md5() Obfuscate Base64 and Sha1 methods
- Improved performance of Obfuscate() so it outperforms "ToBase64()" if input is less than roughly ~400kb
- Updated docs

#### Version history
- View git history of this file if interested

## Installation
https://systemlibrary.github.io/systemlibrary-common-net/Install.html

## Docs
Documentation with code samples:  
https://systemlibrary.github.io/systemlibrary-common-net/

## Nuget
https://www.nuget.org/packages/SystemLibrary.Common.Net/

## Source
https://github.com/systemlibrary/systemlibrary-common-net

## Suggestions and feedback
support@systemlibrary.com

## Lisence
- Free