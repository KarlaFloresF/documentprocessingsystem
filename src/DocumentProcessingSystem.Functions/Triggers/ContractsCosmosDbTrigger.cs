using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Domain.Configurations;
using DocumentProcessingSystem.Domain.Entities;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interfaces;
using DocumentProcessingSystem.Infrastructure.ProcessingDocumentApi.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DocumentProcessingSystem.Functions.Triggers;

public class ContractsCosmosDbTrigger
{
    private readonly ILogger<ContractsCosmosDbTrigger> _logger;
    private readonly IContractTemplateService _contractTemplateService;
    private readonly ICosmosDocumentService _cosmosDocumentService;
    private readonly IDocumentSenderService _documentSenderService;
    private readonly CosmosDbSettings _cosmosDbSettings;


    public ContractsCosmosDbTrigger(
        ILogger<ContractsCosmosDbTrigger> logger,
        IContractTemplateService contractTemplateService,
        ICosmosDocumentService cosmosDocumentService,
        IDocumentSenderService documentSenderService,
        IOptions<CosmosDbSettings> cosmosSettings)
    {
        _logger = logger;
        _contractTemplateService = contractTemplateService;
        _cosmosDocumentService = cosmosDocumentService;
        _documentSenderService = documentSenderService;
        _cosmosDbSettings = cosmosSettings.Value;
    }

    [Function("ContractCosmosDbTrigger")]
    public async Task Run([CosmosDBTrigger(
        databaseName: "%CosmosDb:DatabaseName%",
        containerName: "%CosmosDb:ContractContainerName%",
        Connection = "CosmosDb:ConnectionString",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<GenericTypeDocument> documents)
    {
        if (documents != null && documents.Count > 0)
        {
            foreach (var doc in documents)
            {
                try
                {
                    JObject entityObject = null;

                    if (doc.Content is JsonElement jsonElement &&
                        jsonElement.TryGetProperty("entity", out var entityElement))
                    {
                        var entityContent = entityElement.GetRawText();
                        entityObject = JObject.Parse(entityContent);
                    }
                    
                    var result = await _documentSenderService.SendContractAsync(doc.PartitionKey, entityObject);

                    if (result)
                    {
                        _logger.LogInformation("Contract with partitionKey was successfully processed. {partitionKey}", doc.PartitionKey);
                    }
                    else
                    {
                        _logger.LogError("Contract with partitionKey {partitionKey} failed.", doc.PartitionKey);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing document with Id: " + doc.Id);
                }
            }
        }
    }
}