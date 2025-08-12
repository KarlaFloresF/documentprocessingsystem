using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Application.Services;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Implementation;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interface;
using DocumentProcessingSystem.Infrastructure.TemplateEngine.Implementation;
using DocumentProcessingSystem.Infrastructure.TemplateEngine.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddScoped<IContractTemplateService, ContractTemplateService>();
builder.Services.AddScoped<ILiquidTemplateTransformer, LiquidTemplateTransformer>();
builder.Services.AddSingleton<ICosmosDocumentService, CosmosDocumentService>();

builder.Build().Run();
