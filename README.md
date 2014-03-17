Intention: Less code developing against storage and caching

* Intended to be somewhat opinionated. (ex. Async by default)
* Codebase will often be newer than [NuGet](https://www.nuget.org/packages/AzureDataEaseOfUse) version (documentation follows NuGet version)
* Try to implement [best practices](https://github.com/WindowsAzure-Contrib/AzureDataEaseOfUse/blob/master/BestPractices.md)


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


Guides for
----------

1. [Tables](Tables.md)
2. Blob // Future
3. Queue // Future
4. Cache // Future


Some design goals
-----------------

1. Auto-provisioned batch for Tables (Flywheel)
2. Best practices (UseNagleAlgorithm, Expect100Continue, DefaultConnectionLimit)
3. Transaction throughput can fully saturate limits
4. Storage-Backed caching
5. Table & Blob caching
6. Multi-account management (scale-out performance)
7. Tiered caching (local [fast], Azure.Cache [distributed], Azure.Storage.Blob [plenty of space])
8. Short-cuts for common actions
9. Secondary indexes on Table





