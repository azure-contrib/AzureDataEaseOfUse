Intention: Less code developing against storage and caching

* Intended to be somewhat opinionated.
* Mostly Async since all of the interaction is with a web api.
* Codebase will often be newer than [NuGet](https://www.nuget.org/packages/AzureDataEaseOfUse) version (documentation follows NuGet version)


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
---------

1. [Tables](Tables.md)
2. Blob // Future
3. Queue // Future
4. Cache // Future


Best Practices
--------------


All connections automatically use:

* UseNagleAlgorithm = false;
* Expect100Continue = false;
* DefaultConnectionLimit = 1000; (Best Practice is 100+)

Per best practices communicated in (http://www.microsoftvirtualacademy.com/colleges/windows-azure-deep-dive) 

**You "can" override best practices**

```csharp 
    azure.NagleAlgorithm(enabled: Your_Answer);
    
    azure.Expect100Continue(enabled: Your_Answer);
    
    // Shortcut to do whatever you want to the endpoint
    azure.TableServicePoint();
```








