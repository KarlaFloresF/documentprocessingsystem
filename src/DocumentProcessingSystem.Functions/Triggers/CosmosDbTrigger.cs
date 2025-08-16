using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Domain.Configurations;
using DocumentProcessingSystem.Domain.Entities;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace DocumentProcessingSystem.Functions.Triggers;

public class CosmosDbTrigger
{
    private readonly ILogger<CosmosDbTrigger> _logger;
    private readonly IContractTemplateService _contractTemplateService;
    private readonly ICosmosDocumentService _cosmosDocumentService;
    private readonly CosmosDbSettings _cosmosDbSettings;


    public CosmosDbTrigger(
        ILogger<CosmosDbTrigger> logger,
        IContractTemplateService contractTemplateService,
        ICosmosDocumentService cosmosDocumentService,
        IOptions<CosmosDbSettings> cosmosSettings)
    {
        _logger = logger;
        _contractTemplateService = contractTemplateService;
        _cosmosDocumentService = cosmosDocumentService;
        _cosmosDbSettings = cosmosSettings.Value;
    }

    [Function("ContractCosmosDbTrigger")]
    public async Task Run([CosmosDBTrigger(
        databaseName: "%DataBaseName%",
        containerName: "%RawContractDocumentsContainer%",
        Connection = "CosmosDBConnection",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<RawContractDocument> documents)
    {
        if (documents != null && documents.Count > 0)
        {
            foreach (var doc in documents)
            {
                try
                {
                    var transformedContractDoc = await _contractTemplateService.TransformDocumentFromLiquidTemplateAsync(doc);

                    if (!string.IsNullOrWhiteSpace(transformedContractDoc))
                    {
                        _logger.LogInformation("Contract generated successfully for: " + doc.ClientName);

                        var document = JObject.Parse(transformedContractDoc) ?? throw new NotImplementedException($"Error converting payload {transformedContractDoc} to JObject.");
                        var partitionKey = document["partitionKey"];
                        var contentJson = document["content"];

                        var contract = new GenericTypeDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            PartitionKey = partitionKey.ToString(),
                            Content = contentJson
                        };

                        //TODO: Validate if exists
                        await _cosmosDocumentService.SaveAsync(contract, contract.PartitionKey, _cosmosDbSettings.ContractContainerName);
                    }
                    else
                    {
                        _logger.LogWarning("Building document failed for: " + doc.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating document for: " + doc.Id);
                }
            }
        }
    }
}