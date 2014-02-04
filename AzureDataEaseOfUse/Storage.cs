using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;


namespace AzureDataEaseOfUse
{
    public static class Storage
    {

        // Note: This is lazy loaded
        static Storage()  { SetupBestPractices(); }


        public const string DefaultStorageConnectionStringName = "DefaultStorageConnectionString";
        
        /// <summary>
        /// Creates account object.  Connection strings, NOT app settings, are used for security purposes.
        /// </summary>
        /// <param name="connectionStringName">Name of connection string, not connection string itself.</param>
        public static CloudStorageAccount Connect(string connectionStringName = DefaultStorageConnectionStringName)
        {
            var connection_string = CloudConfigurationManager.GetSetting(connectionStringName);
            //var connection_string = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            var account = CloudStorageAccount.Parse(connection_string);

            return account;
        }

        #region Setup Best Practices (Per AzureWeek)

        /// <summary>
        /// Disabled Nagle. Disables Expect100Continue. Increases DefaultConnectionLimit to 1000.
        /// Executes automatically on app start
        /// </summary>
        public static void SetupBestPractices()
        {
            NagleAlgorithm(false);
            Expect100Continue(false);
            DefaultConnectionLimit(1000);
        }

        #endregion

        #region Setting Overrides

        /// <summary>
        /// Overrides the Best Practice (Disable for small messages, less than 1400 b) already set for this connection
        /// </summary>
        public static CloudStorageAccount NagleAlgorithm(this CloudStorageAccount account, bool enabled)
        {
            // Disable Nagle Algorithm: http://robertgreiner.com/2012/06/why-is-azure-table-storage-so-slow/

            account.TableServicePoint().UseNagleAlgorithm = enabled;

            return account;
        }

        /// <summary>
        /// [Global] Overrides the Best Practice (Disable for small messages, less than 1400 b) already set for this connection
        /// </summary>
        public static void NagleAlgorithm(bool enabled)
        {
            // Disable Nagle Algorithm: http://robertgreiner.com/2012/06/why-is-azure-table-storage-so-slow/

            ServicePointManager.UseNagleAlgorithm = enabled;
        }


        /// <summary>
        /// Overrides the Best Practice (aka Disabled) already set for this connection
        /// </summary>
        public static CloudStorageAccount Expect100Continue(this CloudStorageAccount account, bool enabled)
        {
            account.TableServicePoint().Expect100Continue = enabled;

            return account;
        }

        /// <summary>
        /// [Global] Overrides the Best Practice (aka Disabled) already set for this connection
        /// </summary>
        public static void Expect100Continue(bool enabled)
        {
            ServicePointManager.Expect100Continue = enabled;
        }

        /// <summary>
        /// [Global] Overrides the Best Practice (aka 100+).
        /// </summary>
        public static void DefaultConnectionLimit(int limit)
        {
            ServicePointManager.DefaultConnectionLimit = limit;
        }

        #endregion

        #region Infrastructure (ServicePoint)

        public static ServicePoint TableServicePoint(this CloudStorageAccount account)
        {
            return ServicePointManager.FindServicePoint(account.TableEndpoint);
        }

        public static TableKeys GetTableKeys(this IAzureStorageTable table)
        {
            return new TableKeys(table.GetPartitionKey(), table.GetRowKey());
        }

        #endregion

    }
}
