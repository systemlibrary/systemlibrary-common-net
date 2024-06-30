# Installation

## Requirements
* &gt;= .NET 7

## Install nuget package
* Open your project/solution in Visual Studio
* Open Nuget Project Manager
* Search SystemLibrary.Common.Net
* Install SystemLibrary.Common.Net

## First time usage
- Classes and methods can be used out of the box by including the namespace they live in

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
* Below are all default and modifiable configurations for this package

###### appSettings.json:
```json  
	{
		"systemLibraryCommonNet": {
			"debug": false,
			"dump": {
				"folder": "%HomeDrive%\\Logs\\",
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
