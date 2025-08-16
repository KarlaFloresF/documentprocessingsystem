using DotLiquid;

namespace DocumentProcessingSystem.Infrastructure.TemplateEngine.Interfaces
{
    public interface ILiquidTemplateTransformer 
    {
        Task<string?> ConvertDocumentUsingLiquidTemplateAsync(Hash hash, string templateName);
    }
}
