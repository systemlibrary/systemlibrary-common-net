# Installation

## Requirements
* &gt;= .NET 8

## Install nuget package
* Open your project/solution in Visual Studio
* Open Nuget Project Manager
* Search SystemLibrary.Common.Net
* Install SystemLibrary.Common.Net

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
