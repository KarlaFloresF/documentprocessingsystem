using DocumentProcessingSystem.Infrastructure.TemplateEngine.Interface;
using DotLiquid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DocumentProcessingSystem.Infrastructure.TemplateEngine.Implementation
{
    public class LiquidTemplateTransformer : ILiquidTemplateTransformer
    {
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public LiquidTemplateTransformer(ILoggerFactory logger, IMemoryCache memoryCache)
        {
            _logger = logger.CreateLogger<LiquidTemplateTransformer>();
            _cache = memoryCache;
        }
        public async Task<string?> ConvertDocumentUsingLiquidTemplateAsync(Hash hash, string templateName)
        {
            try
            {
                Template? parser = await _cache.GetOrCreateAsync(templateName!, async entry =>
                {
                    var source = string.Empty;
                    //TODO: Look for the liquid template in a BLOB Container
                    using (var streamReader = new StreamReader($"C:\\LiquidTemplates\\{templateName}"))
                    {
                        source = streamReader.ReadToEnd();
                    }
                    
                    if (!string.IsNullOrWhiteSpace(source))
                    {
                        parser = Template.Parse(source);
                        parser.MakeThreadSafe();
                        return parser;
                    }
                    else
                    {
                        throw new ArgumentNullException(templateName, "Liquid template was empty or not found in blob storage");
                    }
                });
                if (parser != null)
                {
                    var ret = parser.Render(hash);
                    return ret;
                }
                else
                {
                    throw new ArgumentNullException(templateName, "Liquid template was null from both storage and cache");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{className}: An error occurred mapping a liquid template file ({templateName}). Exception: {exMgs}", nameof(LiquidTemplateTransformer), templateName, ex.Message);
                return null;
            }
        }
    }
}
