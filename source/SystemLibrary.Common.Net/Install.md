# Installation

## Requirements
* .NET &gt;= 6.0

## Install nuget package

* Open your project/solution in Visual Studio
* Open Nuget Project Manager
* Search and install SystemLibrary.Common.Net

## First time usage

- Classes and methods can be used out of the box by including the namespace they live in

- Sample:
```csharp  
	public class Car 
	{
		public void Test() 
		{
			var s = "";
			var b = s.IsNot(); //IsNot() is an extension method in this package living in the global namespace
								 //In this case the method IsNot returns true
		}
	}
```

## Package Configurations
* Default and modifiable configurations for this package:

appSettings.json:
```json  
	{
		"systemLibraryCommonNet": {
			"dump": {
				"folder": "%HomeDrive%\\Logs\\",
				"fileName": "SysLib.log"
			},
			"json": {
				"maxDepth": 32,
				"propertyNameCaseInsensitive": true,
				"writeIndented": false,
				"allowTrailingCommas": true,
				"readCommentHandling": "Allow"
			}
		}
	}
```  
