Best Practices
--------------

Communicated in (http://www.microsoftvirtualacademy.com/colleges/windows-azure-deep-dive) 

All connections automatically use:

* UseNagleAlgorithm = false;
* Expect100Continue = false;
* DefaultConnectionLimit = 1000; (Best Practice is 100+)

**You "can" override best practices**

```csharp 
    azure.NagleAlgorithm(enabled: Your_Answer);
    
    azure.Expect100Continue(enabled: Your_Answer);
    
    // Shortcut to do whatever you want to the endpoint
    azure.TableServicePoint();
```
