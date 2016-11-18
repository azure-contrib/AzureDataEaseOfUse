Intention: Less code developing against storage and caching

* Intended to be somewhat opinionated. (ex. Async by default)
* Codebase will often be newer than [NuGet](https://www.nuget.org/packages/AzureDataEaseOfUse) version (documentation follows NuGet version)
* Try to implement [best practices](https://github.com/WindowsAzure-Contrib/AzureDataEaseOfUse/blob/master/BestPractices.md)

***Several updates to usage style are in process while trying to move projects towards v1


MIT Licensed. As with any code, use in your own good judgment and have fun :-)

Support
-------

Questions, comments, or features give me a buzz [@rskopecek](https://twitter.com/rskopecek)

Issues: [Submit an issue](https://github.com/WindowsAzure-Contrib/AzureDataEaseOfUse/issues)


Getting Started
---------------

Install from [NuGet](https://www.nuget.org/packages/AzureDataEaseOfUse):
```
PM> Install-Package AzureDataEaseOfUse -Pre
```

Add AppSetting

```csharp
<add key ="DefaultStorageConnectionString" value ="DefaultEndpointsProtocol=https;AccountName=[name];AccountKey=[key];"/>
```






