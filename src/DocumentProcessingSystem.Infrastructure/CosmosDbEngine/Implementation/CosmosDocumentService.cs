using DocumentProcessingSystem.Domain.Configurations;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Implementation
{
    public class CosmosDocumentService : ICosmosDocumentService
    {
        private readonly CosmosClient _client;
        private readonly string _databaseName;

        public CosmosDocumentService(CosmosClient client, IOptions<CosmosDbSettings> settings)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));

            var config = settings.Value;
            if (string.IsNullOrWhiteSpace(config.DatabaseName))
                throw new ArgumentException("DatabaseName is required in CosmosDbSettings.");

            _databaseName = config.DatabaseName;
        }

        public async Task SaveAsync<T>(T document, string partitionKey, string containerName)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (string.IsNullOrWhiteSpace(partitionKey))
                throw new ArgumentException("PartitionKey is required.");

            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("ContainerName is required.");

            var container = _client.GetContainer(_databaseName, containerName);
            await container.UpsertItemAsync(document, new PartitionKey(partitionKey));
        }
    }
}
