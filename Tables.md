Getting Started: [GoTo ReadMe.md](https://github.com/WindowsAzure-Contrib/AzureDataEaseOfUse/blob/master/README.md)

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
