using Newtonsoft.Json.Linq;

namespace DocumentProcessingSystem.Infrastructure.ProcessingDocumentApi.Interfaces
{
    public interface IDocumentSenderService
    {
        Task<bool> SendContractAsync(string partitionKey, JObject content);
    }
}