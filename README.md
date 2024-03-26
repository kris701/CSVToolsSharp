
<p align="center">
    <img src="https://github.com/kris701/CSVToolsSharp/assets/22596587/9251d9de-632b-41c5-a1da-31750cd24384" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/CSVToolsSharp/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/CSVToolsSharp/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/CSVToolsSharp)
![Nuget](https://img.shields.io/nuget/dt/CSVToolsSharp)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/CSVToolsSharp/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/CSVToolsSharp)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)

CSV Tools Sharp is a little project to manipulate and output CSV files.
You can find it on the [NuGet Package Manager](https://www.nuget.org/packages/CSVToolsSharp/) or the [GitHub Package Manager](https://github.com/kris701/CSVToolsSharp/pkgs/nuget/CSVToolsSharp).

# How to Use
The package is inspired by that of [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/9.0.0-preview.2.24128.5), where you can access two primary static methods, `CSVSerialiser.Deserialise` and `CSVSerialiser.Serialise` to convert generic classes into CSV format and back.
You have to give the properties of the CSV serialisable classes a `CSVColumn` attribute for them to be targeted for serialisation.
You can also pass a `CSVSerialiserOptions` object to the serialisation/deserialisation for more settings.

## Example
Class to serialise/deserialize:
```csharp
public class TestClass
{
    [CSVColumn("Column1")]
    public string Value { get; set; }
}
```
You can then use the serialiser and deserialiser as follows:
```csharp
var csvText = CSVSerialiser.Serialise(new List<TestClass>(){ new TestClass(){ Value = "abc" } });
```
Gives
```csv
Column1
abc
```

## Example
Class to serialise/deserialize:
```csharp
public class TestClass2
{
    [CSVColumn("Column1")]
    public string Value { get; set; }
    [CSVColumn("Column 2")]
    public string Value2 { get; set; }
}
```
You can also make the CSV print more readably:
```csharp
var csvText = CSVSerialiser.Serialise(
    new List<TestClass2>(){ 
        new TestClass2(){ Value = "asdafaseasasd", Value2 = "abc" } 
    }, new CSVSerialiserOptions(){ PrettyOutput = true });
```
Gives
```csv
Column1      ,Column 2
asdafaseasasd,abc
```