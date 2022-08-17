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
- 6.2.0.2
- 'AddEnvironmentVariables()' is always invoked if the configuration files are .xml (.xml loading threw exception if it isnt invoked), else its never called
- Custom configuration files no longer adds "environmentVariables", hence 'userName', and other environment variable names, are now allowed to be used in your custom files. If 'UserName' is used inside 'AppSettings.cs' then that will be read from environment, so it will return your login name on your computer, always (Breaking change)
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