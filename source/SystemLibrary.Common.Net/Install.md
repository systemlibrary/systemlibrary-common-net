# Installation

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
			var b = s.IsNot(); //IsNot() is one extension in this package, returns true in this case
		}
	}
```

## Package Configurations
* Default (and modifiable) configurations in this package:

appSettings.json:
```json  
	{

		"systemLibraryCommonNet": {
			"dump": {
				"folder": "C:\\logs\\",
				"fileName": "SysLib.log"
			}
		}
	}
```  
