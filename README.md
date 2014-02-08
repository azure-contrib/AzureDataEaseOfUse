Intention: Less code developing against storage and caching

* Intended to be somewhat opinionated, but allow overrides for most alterations
* Currently only Tables are developed against, and still have a bit of work to go.
* Most all methods are Async since all of the interaction is with a web api.
* A synchronous namespace will be added to add ease of use
* Most all functionality is currently "extended", not created (batches are the exception). This way you don't lose much of the bare metal control. Just gain simplicity.
* Tests run against live Azure

MIT Licensed. As with any code, use in your own good judgment and have fun :-)



Getting Started
==========

Install from NuGet:
```
PM> Install-Package AzureDataEaseOfUse -Pre
```

Add AppSetting

```csharp
<add key ="DefaultStorageConnectionString" value ="DefaultEndpointsProtocol=https;AccountName=[name];AccountKey=[key];"/>
```

Add namespaces

```csharp    
using AzureDataEaseOfUse;
using AzureDataEaseOfUse.Tables; //temp: needed for batches
using AzureDataEaseOfUse.Tables.Async;
```

Your objects

```csharp 
class Mine : TableEntity, IAzureStorageTable
{
    // your stuff

    public string GetPartitionKey() {}
    public string GetRowKey() {}
}
```

Connect to Azure Storage

```csharp 
// Kick off will change in the future, but nothing major
var azure = Storage.Connect();
    
// or override connection string name
var azure = Storage.Connect("my_specific_name");
```

rock and roll

Aww Crud
-----------
```csharp 
    var table = await azure.Table("MyTable");

    await table.Add(stuff);
    
    await table.Get<ExamplePost>("partition", "row");
    
    await table.Update(stuff);
     
    await table.Delete(stuff);
```
or 1 line add (or update/delete/etc)
```csharp
    await Storage.Connect().Table("Posts").Add(entry);
```

List and Search
---------------
```csharp 
    // Everything in a partition
    table.List<ExamplePost>("partition");

    // Search shortcut
    table.Where<ExamplePost>(q => q.BlogId == "MinePlease");
```

Batches
-------

Most batch commands return the batch object to allow declarative continuation.

```csharp 
    var batch = table.Batch<ExamplePost>();
    
    batch.Add(stuff).Update(stuff2).Delete(stuff3).Get("partition","row").Execute();
    
    batch.Add(stuff).Update(stuff2).ExecuteAsync();
```

Batches auto-provision TableBatchOperations under the hood, by partition key and 100 count.  Aka encapsulating you, a bit, from some mechanics of Azure Storage Tables.
```csharp 
    for (int x = 1; x < N; x++)
      batch.Add(random_Stuff_For_Random_Blog);

    batch.ExecuteAsync();
    
    batch.Execute();
```

Tables
-----------

```csharp 

    // Get and create
    azure.Table("MyTable");
    
    // Get, but don't create
    azure.Table("MyTable", createIfNotExists: false);

    // List all tables
    azure.Tables();

    azure.Tables("prefix");

```

Best Practices
--------------


All connections automatically use:

* UseNagleAlgorithm = false;
* Expect100Continue = false;
* DefaultConnectionLimit = 1000; (Best Practice is 100+)

Per best practices communicated in (http://www.microsoftvirtualacademy.com/colleges/windows-azure-deep-dive) 


You "can" override best practices
-------------------

Per Connection
```csharp 
    azure.NagleAlgorithm(enabled: Your_Answer);
    
    azure.Expect100Continue(enabled: Your_Answer);
    
    // Shortcut to do whatever you want to the endpoint
    azure.TableServicePoint();
```

App Globablly
```csharp     
    Storage.NagleAlgorithm(enabled: Your_Answer);
    
    Storage.Expect100Continue(enabled: Your_Answer);
    
    Storage.DefaultConnectionLimit(limit: Your_Answer);
```







