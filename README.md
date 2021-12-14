# webenology.blazor.components
Blazor Component Library

Nuget package: https://www.nuget.org/packages/webenology.blazor.components/

## How to Use Generate PDF From Blazor Component
In Startup make sure to add in 
```
services.AddWebenologyHelpers();
```

On Your component make sure to inject the blazor pdf class

```razor
[Inject]
private IBlazorPdf blazorPdf {get;set;}
```

Then to get the pdf in base64 you will call the following, we are generating the Counter component

The Counter paramaers are added in by `x.Add(y=> y.Param, value)`

The Css and Js file locations are absolute locations

```csharp
var fileName = "my file name";
var cssFileLocations = new List<string>();
var jsFileLocations = new List<string>();
_base64Results = blazorPdf.GetBlazorInPdfBase64<Counter>(x => x.Add(y=> y.StartingValue, 2), fileName, cssFileLocations, jsFileLocations);
```
You can render the results however you want.
An easy way to render them is to set the base64 into an embed component

```razor
<embed style="width: 100%; height: 650px;" src="data:application/pdf;base64,@_base64Results">
```