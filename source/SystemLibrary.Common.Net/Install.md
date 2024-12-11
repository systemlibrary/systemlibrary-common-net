# Installation
- Open Project/Solution in Visual Studio
- Open Nuget Project Manager
- Search SystemLibrary.Common.Net
- Install SystemLibrary.Common.Net

[![Latest version](https://img.shields.io/nuget/v/SystemLibrary.Common.Net)](https://www.nuget.org/packages/SystemLibrary.Common.Net)

## Requirements
- &gt;= .NET 8

## First time usage
- String extensions and Dump can be used out of the box as they are in the global namespace
- Other classes and methods can be used by using the namespace they live in

- Sample:
```csharp  
	public class Car 
	{
		public void Test() 
		{
			var s = "";
			var b = s.IsNot();	//IsNot() is a method from this package living in the global namespace
								//Is true in this case, as null, "" and " " returns true
		}
	}
```

## Package Configurations
* All the default and configurable settings for this package.

###### appSettings.json:
```json  
	{
		"systemLibraryCommonNet": {
			"debug": false,
			"dump": {
				"folder": "%HomeDrive%/Logs/",
				"fileName": "DumpWrite.log",
			},
			"json": {
				"allowTrailingCommas": true,
				"maxDepth": 32,
				"propertyNameCaseInsensitive": true,
				"readCommentHandling": "Skip",
				"jsonIgnoreCondition": "WhenWritingNull"
				"writeIndented": false
			}
		}
	}
```
