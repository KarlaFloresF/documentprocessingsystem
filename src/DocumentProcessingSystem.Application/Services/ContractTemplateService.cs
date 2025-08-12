using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Domain.Entities;
using DocumentProcessingSystem.Infrastructure.TemplateEngine.Interface;
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

        public async Task<string?> GenerateContractAsync(RawContractDocument rawContract)
        {
            var templateName = rawContract.DocumentType switch
            {
                "Contract" => "ContractTemplate.liquid",
                "Sale" => "SaleTemplate.liquid",
                _ => throw new ArgumentException("Unsupported document type")
            };

            var hash = Hash.FromAnonymousObject(rawContract);
            var result = await _liquidTemplateTtransformer.ConvertDocumentUsingLiquidTemplateAsync(hash, templateName);

            return result;
        }

    }
}
