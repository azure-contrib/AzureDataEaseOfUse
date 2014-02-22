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

Flywheel (aka managed batch)
----------------------------

* Auto partitions into batches
* Auto executes/flushes batches when full as Async Task to prevent blocking
* Processes results on flush (blocking)

```csharp 
    var flywheel = table.Flywheel<ExamplePost>();

    for (int x = 1; x < N; x++)
    {
        flywheel.Insert(stuff);
        flywheel.Replace(even_more_stuff);
        flywheel.Delete(hording_lots_of_stuff);

        flywheel.InsertOrReplace(stuff);
        flywheel.Merge(stuff);
        flywheel.InsertOrMerge(stuff);

        // auto flushes every time a [partition] batch is full
    }

    flywheel.Flush(); // sends unfull batches. waits for results. processes outcomes
```

Outcomes

```csharp 

    // Reflect on outcomes
    if(flywheel.HasErrors)
        DoSomethingWith(flywheel.Errors);
        
    flywheel.SuccessCount;    
    
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
