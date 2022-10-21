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
- 6.4.1.3
- Added example comments
- Added Md5 extension methods for stream, byte[] and string 
- Added Sha1 extension methods for stream, byte[] and string
- Added Obfuscate and Deobfuscate extension method for string
- Setting deps versions from [6, 1000) due to this is "base .NET functionality" and should most likely work in v7, 8 ...
  - Do note "most likely work", who knows what will break in version 23 or 64... in theory it will work forever
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