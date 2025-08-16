using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Domain.Configurations;
using DocumentProcessingSystem.Domain.Entities;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DocumentProcessingSystem.Functions.Triggers;

public class ContractsCosmosDbTrigger
{
    private readonly ILogger<ContractsCosmosDbTrigger> _logger;
    private readonly IContractTemplateService _contractTemplateService;
    private readonly ICosmosDocumentService _cosmosDocumentService;
    private readonly CosmosDbSettings _cosmosDbSettings;


    public ContractsCosmosDbTrigger(
        ILogger<ContractsCosmosDbTrigger> logger,
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
                    //TODO: CALL API SERVICE TO SEND TO DB
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing document with Id: " + doc.Id);
                }
            }
        }
    }
}