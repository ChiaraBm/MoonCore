# Getting Started

## Installing
Install MoonCore into your project using the .NET CLI
````shell
dotnet add package MoonCore 
````

Now you are ready to use the MoonCore utilities of the main package.
As an example you can try to convert a number of bytes to a user friendly
text representation using the formatter.

````csharp
using MoonCore.Helpers;

Console.WriteLine(Formatter.FormatSize(1024)); // "1 KB"
````