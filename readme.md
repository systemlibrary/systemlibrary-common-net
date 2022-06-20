# SystemLibrary Common Net

## Description
A library of classes and methods for any .NET &gt;= 5 application
- Common useful extensions for strings, arrays, ...
- Custom config class by inheriting Config&lt;&gt; and it auto-reads your json config file into the C# class
- Dump.Write() equivalent to Console.Log in javascript
- Convert to/from Json through method ".ToJson()" on any object
- Convert parts of a json response to a C# class through .PartialJson() extension method
- Fire and Forget through Async.Run()

## Requirements
- &gt;= .NET 5

## Latest Version
- EnvironmentConfig.Name is now rewritten and rethought
- EnvironmentConfig.Name do not read launchSettings anymore, nor "ASPNETCORE_ENV", nor "DOTNET_ENVIRONMENT"
- EnvironmentConfig.Name reads only "ASPNETCORE_ENVIRONMENT" variable passed to your .NET Core application (unit test, console, webapp...)
- EnvironmentConfig.Name has a good description on how to use it in the docs, so check the 'docs link' below

## Version history
- View git history of this file if interested

## Docs
Documentation with samples:  
https://systemlibrary.github.io/systemlibrary-common-net/

## Nuget
https://www.nuget.org/packages/SystemLibrary.Common.Net/

## Suggestions and feedback
support@systemlibrary.com

## Lisence
- It's free forever, copy paste as you'd like