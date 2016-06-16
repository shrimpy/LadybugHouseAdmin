using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DonationPage.Models
{
    public class TableManager<T> where T : ITableEntity, new()
    {
        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable cloudTable;
        private string tableName;

        public TableManager(string tableName)
        {
            this.tableName = tableName;

            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            tableClient = storageAccount.CreateCloudTableClient();

            cloudTable = tableClient.GetTableReference(this.tableName);
            cloudTable.CreateIfNotExists();
        }

        public TableManager() : this(typeof(T).Name)
        {
        }

        public Task CreateEntityAsync(T entity)
        {
            CloudTable table = tableClient.GetTableReference(tableName);
            TableOperation insertOperation = TableOperation.Insert(entity);
            return table.ExecuteAsync(insertOperation);
        }

        public async Task<List<T>> GetAllEntitiesAsync()
        {            
            TableContinuationToken continuationToken = null; // Start from the beginning of the table
            var query = new TableQuery<T>();
            var result = new List<T>();
            TableQuerySegment<T> segment = null;

            do {
                segment = await this.cloudTable.ExecuteQuerySegmentedAsync<T>(query, continuationToken);
                if (segment == null) {
                    continuationToken = null;
                    break;
                }

                result.AddRange(segment);
                continuationToken = segment.ContinuationToken;
            }
            while (continuationToken != null);

            return result;
        }

        public T GetEntity(string partitionKey, string rowKey)
        {
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = cloudTable.Execute(operation);

            return (T) result.Result;
        }

        public Task UpsertEntity(T entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);
            return cloudTable.ExecuteAsync(operation);
        }

        public Task DeleteEntity(T entity)
        {
            var operation = TableOperation.Delete(entity);
            return cloudTable.ExecuteAsync(operation);
        }
    }
}