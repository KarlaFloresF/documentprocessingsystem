using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessingSystem.Infrastructure.TemplateEngine.Interface
{
    public interface ILiquidTemplateTransformer
    {
        Task<string?> ConvertDocumentUsingLiquidTemplateAsync(Hash hash, string templateName);
    }
}
