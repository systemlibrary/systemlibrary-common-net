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
- 6.10.0.1
- EnvironmentConfig class is changed, it now takes another generic param (Enum of 'environment names') (breaking change)
- ConfigLoader now re-tries if crashing, by creating the Config default first time (static member initialization fails. Static members get method is actually invoked)
- EnvironmentConfig.Current.IsProd => EnvironmentConfig.IsProd (breaking change)
- EnvironmentConfig.Current.IsLocal => EnvironmentConfig.IsLocal (breaking change)
- New public var: EnvironmentCOnfig.EnvironmentName is now converted 'once' during 'Name set', stored as static Enum (optimization)
 

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