namespace DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interfaces
{
    public interface ICosmosDocumentService
    {
        Task SaveAsync<T>(T document, string partitionKey, string containerName);
    }
}
