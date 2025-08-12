using DocumentProcessingSystem.Domain.Entities;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Implementation
{
    public class CosmosDocumentService : ICosmosDocumentService
    {
        private readonly CosmosClient _client;
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public CosmosDocumentService(IConfiguration configuration)
        {
            var connectionString = configuration["CosmosDBConnection"];
            var databaseName = configuration["DataBaseName"];
            var containerName = configuration["ContractsContainer"];

            _client = new CosmosClient(connectionString);
            _container = _client.GetContainer(databaseName, containerName);
        }

        public async Task SaveAsync<T>(DocumentWrapper<T> document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (string.IsNullOrWhiteSpace(document.PartitionKey))
                throw new ArgumentException("PartitionKey is required.");

            await _container.UpsertItemAsync(document, new PartitionKey(document.PartitionKey));
        }
    }
}
