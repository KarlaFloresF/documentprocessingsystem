using DocumentProcessingSystem.Domain.Entities;

namespace DocumentProcessingSystem.Application.Interfaces
{
    public interface IContractTemplateService
    {
        Task<string?> TransformDocumentFromLiquidTemplateAsync(RawContractDocument rawContract);
    }
}
