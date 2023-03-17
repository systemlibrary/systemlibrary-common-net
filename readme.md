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
- 6.6.1.5
- Enum.ToValue() extension can now return null if the EnumValue attribute has a value of null or if Enum passed is (Enum)null
- Enum.ToText() extension can now return null if the EnumText attribute has a value of null or if Enum passed is (Enum)null
- Enum.ToEnumText() extension can now return null if the EnumText attribute do not exist or its value is null, or Enum passed is (Enum)null
- Enum.ToEnumValue() extension can now return null if the EnumValue attribute do not exist or its value is null, or Enum passed is (Enum)null

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