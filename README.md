AzureDataEaseOfUse
==================

Intention: Less code developing with Azure storage and caching

Subtext: Intended to be somewhat opinionated, but allow overrides for most alterations

Currently only Tables are developed against, and still have a bit of work to go. After more work, a NuGet package will probably be created. Most all methods are synchronous except batch. It should change in the future to be async first as it is the appropriate style for api calls. However, it is more common for devs to use sync methods as it is more easily understood. Sync/Async will be split up by namespace to remove the "Async" postpended naming clutter.

Most all functionality is "extended", not created (batches are the exception). This way you don't lose much of the bare metal control. Just gain simplicity.

Crud ops are tested against live Azure. Batch and search ops still need vetting.

MIT Licensed. As with any code, use in your own good judgment and have fun :-)



Examples
==========

Add namespaces
```csharp    
    using AzureDataEaseOfUse;
    using AzureDataEaseOfUse.Tables;
```

Pretend connect to Azure Storage
```csharp 
    // Kick off will change in the future, but nothing major
    var azure = Storage.Connect();
```

Your objects
```csharp 
    class Mine : TableEntity, IAzureStorageTable
    {
        public string GetPartitionKey() {}
        public string GetRowKey() {}
    }
```

Simple CRUD
-----------
```csharp 
    azure.Table("MyTable").Add(stuff);
    
    azure.Table("MyTable").Get<ExamplePost>("partition", "row");
    
    azure.Table("MyTable").Update(stuff);
     
    azure.Table("MyTable").Delete(stuff);
```
or save more time
```csharp 
    var table = azure.Table("MyTable");

    table.Add(stuff);
    
    table.Get<ExamplePost>("partition", "row");
    
    table.Update(stuff);
     
    table.Delete(stuff);
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
    
    
    // Get Partition and Row keys in 1 shot
    table.GetTableKeys();
    
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
    
    // Shortcut do whatever you want to the endpoint
    azure.TableServicePoint();
```

App Globablly
```csharp     
    Storage.NagleAlgorithm(enabled: Your_Answer);
    
    Storage.Expect100Continue(enabled: Your_Answer);
    
    Storage.DefaultConnectionLimit(limit: Your_Answer);
```







