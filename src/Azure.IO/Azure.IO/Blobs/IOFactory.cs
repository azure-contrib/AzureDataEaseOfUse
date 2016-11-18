using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.IO.Blobs
{
    public static class IOFactory
    {

        public const string StorageEmulatorConnectionString = "UseDevelopmentStorage=true";


        public static Block.IIO GetBlockIO(string containerName, Func<string, string> pathFormatter)
        {
            var connectionString = GetConnectionString(containerName);

            if (connectionString.Equals("memory", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Block.IO(new InMemoryContainer(), pathFormatter);
            }
            else
            {
                return new Block.IO(new AzureContainer(GetContainer(connectionString, containerName)), pathFormatter);
            }
        }


        public static string GetConnectionString(string name)
        {
            string[] scopes = { $"Azure.IO.Blobs:{name}", "Azure.IO.Blobs:Default", "Azure.IO:Default" };

            foreach (var scope in scopes)
            {
                var value = Configuration.ConnectionString(scope);

                if (string.IsNullOrWhiteSpace(value))
                    continue;

                return value.Equals("emulator", StringComparison.InvariantCultureIgnoreCase) ? StorageEmulatorConnectionString : value;
            }

            throw new Exception($"No storage connection string could be found for {name}.  Please fill out one of the following in your configs connection string section: 'Azure.IO.Blobs:{name}', 'Azure.IO.Blobs:Default', or 'Azure.IO:Default'.  Please provide either 'memory', 'emulator', or the exact connection string.");
        }

        public static string GetContainerName(string name)
        {
            var value = Configuration.AppSetting($"Azure.IO.Blobs.ContainerName:{name}");

            return string.IsNullOrWhiteSpace(value) ? name : value;
        }


        private static CloudBlobContainer GetContainer(string connectionString, string containerName)
        {
            var account = CloudStorageAccount.Parse(connectionString);

            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName.ToLowerInvariant());

            container.CreateIfNotExists();

            return container;
        }

    }
}
