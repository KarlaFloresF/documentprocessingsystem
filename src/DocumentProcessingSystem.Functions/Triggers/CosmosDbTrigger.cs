using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Application.Services;
using DocumentProcessingSystem.Domain.Entities;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DocumentProcessingSystem.Functions.Triggers;

public class CosmosDbTrigger
{
    private readonly ILogger<CosmosDbTrigger> _logger;
    private readonly IContractTemplateService _contractTemplateService;
    private readonly ICosmosDocumentService _cosmosDocumentService;

    public CosmosDbTrigger(
        ILogger<CosmosDbTrigger> logger,
        IContractTemplateService contractTemplateService,
        ICosmosDocumentService cosmosDocumentService)
    {
        _logger = logger;
        _contractTemplateService = contractTemplateService;
        _cosmosDocumentService = cosmosDocumentService;
    }

    [Function("ContractCosmosDbTrigger")]
    public async Task Run([CosmosDBTrigger(
        databaseName: "%DataBaseName%",
        containerName: "%RawContractDocumentsContainer%",
        Connection = "CosmosDBConnection",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<RawContractDocument> input)
    {
        if (input != null && input.Count > 0)
        {
            foreach (var contract in input)
            {
                try
                {
                    var transformedContractDoc = await _contractTemplateService.GenerateContractAsync(contract);

                    if (!string.IsNullOrWhiteSpace(transformedContractDoc))
                    {
                        _logger.LogInformation("Contract generated successfully for: " + contract.ClientName);
                        // Aquí podrías guardar el resultado en otro contenedor, blob, etc.
                        //quiero cambiar 
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var wrapper = JsonSerializer.Deserialize<DocumentWrapper<ContractDocument>>(transformedContractDoc, options);
                        if (wrapper?.Content?.Entity != null)
                        {
                            wrapper.Content.Entity.DocumentStatus = "Persisted";
                        }
                        //var persistedContractDoc = JsonSerializer.Serialize(wrapper);
                        await _cosmosDocumentService.SaveAsync(wrapper);
                        //SAVE INTO COSMOSDB
                    }
                    else
                    {
                        _logger.LogWarning("Contract generation failed for: " + contract.ClientName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating contract for: " + contract.ClientName);
                }
            }

        }
    }
}