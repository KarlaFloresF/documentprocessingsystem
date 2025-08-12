using DocumentProcessingSystem.Domain.Entities;

namespace DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interface
{
    public interface ICosmosDocumentService
    {
        Task SaveAsync<T>(DocumentWrapper<T> document);
    }
}
