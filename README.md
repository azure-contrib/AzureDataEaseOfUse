AzureDataEaseOfUse
==================

Intention: Less code developing with Azure storage and caching

Subtext: Intended to be somewhat opinionated, but allow overrides for most alterations

Currently only Tables are developed against, and still have a bit of work to go.  After more work, a NuGet package will probably be created.  Most all methods are synchonous except batch.  It should change in the future to be async first as it is the appropriate style for api calls.  However, it is more common for devs to use sync methods as it is more easily understood.  Sync/Async will be split up by namespace to remove the "Async" postpended naming clutter.

Most all functionality is "extended", not created (batches are the exception).  This way you don't loose much of the baremetal control.  Just gain simplicity.

Crud ops are tested against live Azure.  Batch and search ops still need vetting.


MIT Licensed.  As with any code, use in your own good judgement and have fun :-)



Examples
==========

Add namespaces
    
    using AzureDataEaseOfUse;
    using AzureDataEaseOfUse.Tables;

Pretend connect to Azure Storage

    var azure = Storage.Connect();

Your objects

    class Mine : TableEntity, IAzureStorageTable
    {
        public string GetPartitionKey() {}
        public string GetRowKey() {}
    }



Simple CRUD
-----------

    azure.Table("MyTable").Add(stuff);
    
    azure.Table("MyTable").Get<ExamplePost>("partition", "row");
    
    azure.Table("MyTable").Update(stuff);
     
    azure.Table("MyTable").Delete(stuff);

or save more time

    var table = azure.Table("MyTable");

    table.Add(stuff);
    
    table.Get<ExamplePost>("partition", "row");
    
    table.Update(stuff);
     
    table.Delete(stuff);


List and Search
---------------

    // Everything in a partition
    table.List<ExamplePost>("partition");

    // Search shortcut
    table.Where<ExamplePost>(q => q.BlogId == "MinePlease");
    
List Tables
-----------

    azure.Tables();
    
    azure.Tables("prefix");


Batches
-------
    
    var batch = table.Batch<ExamplePost>();
    
    batch.Add(stuff).Update(stuff2).Delete(stuff3).Get("partition","row").Execute();
    
    batch.Add(stuff).Update(stuff2).ExecuteAsync();


Batches auto-provision TableBatchOperations under the hood, by partition key and 100 count.  Aka encapsulating you, a bit, from some mechanics of Azure Storage Tables.

    for (int x = 1; x < N; x++)
      batch.Add(random_Stuff_For_Random_Blog);

    batch.ExecuteAsync();
    
    batch.Execute();


Best Practices
--------------


All connections automatically set:

* UseNagleAlgorithm = false;
* Expect100Continue = false;
* DefaultConnectionLimit = 1000;

Per best practices communicated in (http://www.microsoftvirtualacademy.com/colleges/windows-azure-deep-dive) 


You "can" override best practices
-------------------

Per Connection

    azure.NagleAlgorithm(enbled: Your_Answer);
    
    azure.Expect100Continue(enbled: Your_Answer);

App Globablly
    
    Storage.NagleAlgorithm(enbled: Your_Answer);
    
    Storage.Expect100Continue(enbled: Your_Answer);
    
    Storage.DefaultConnectionLimit(limit: Your_Answer);
    







