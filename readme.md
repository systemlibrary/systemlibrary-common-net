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
- Multithreaded ToJson() threw exception due to "ReadCommentHandling" were set (its immutable)

## Docs
Documentation with samples:  
https://systemlibrary.github.io/systemlibrary-common-net/

## Nuget
https://www.nuget.org/packages/SystemLibrary.Common.Net/

## Suggestions and feedback
support@systemlibrary.com

## Lisence
- It's free forever, copy paste as you'd like