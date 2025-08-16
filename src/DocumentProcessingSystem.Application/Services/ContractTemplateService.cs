using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Domain.Entities;
using DocumentProcessingSystem.Infrastructure.TemplateEngine.Interfaces;
using DotLiquid;

namespace DocumentProcessingSystem.Application.Services
{
    public class ContractTemplateService : IContractTemplateService
    {
        private readonly ILiquidTemplateTransformer _liquidTemplateTtransformer;

        public ContractTemplateService(ILiquidTemplateTransformer transformer)
        {
            _liquidTemplateTtransformer = transformer;
        }

        public async Task<string?> TransformDocumentFromLiquidTemplateAsync(RawContractDocument rawContract)
        {
            var templateName = rawContract.DocumentType switch
            {
                "Contract" => "ContractTemplate.liquid",
                _ => throw new ArgumentException("Unsupported document type")
            };

            var hash = Hash.FromAnonymousObject(rawContract);
            var result = await _liquidTemplateTtransformer.ConvertDocumentUsingLiquidTemplateAsync(hash, templateName);

            return result;
        }

    }
}
