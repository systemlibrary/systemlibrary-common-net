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
- 6.3.0.2
- Added StreamExtensions with ToJsonAsync so streams can take advantage of the additional json converters inside this package
- Added 3 json converters: int to string, string to Enums, and null values to default Enum value (breaking change as Json serialization/deser has different default options)
- StringBuilder extension fix: TrimEnd and EndsWith() now allows " " (space)
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