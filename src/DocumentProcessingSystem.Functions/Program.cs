using DocumentProcessingSystem.Application.Interfaces;
using DocumentProcessingSystem.Application.Services;
using DocumentProcessingSystem.Domain.Configurations;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Implementation;
using DocumentProcessingSystem.Infrastructure.CosmosDbEngine.Interfaces;
using DocumentProcessingSystem.Infrastructure.ProcessingDocumentApi.Implementation;
using DocumentProcessingSystem.Infrastructure.ProcessingDocumentApi.Interfaces;
using DocumentProcessingSystem.Infrastructure.TemplateEngine.Implementation;
using DocumentProcessingSystem.Infrastructure.TemplateEngine.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddScoped<IContractTemplateService, ContractTemplateService>();
builder.Services.AddScoped<ILiquidTemplateTransformer, LiquidTemplateTransformer>();
builder.Services.AddSingleton<ICosmosDocumentService, CosmosDocumentService>();

builder.Services.Configure<CosmosDbSettings>(
    builder.Configuration.GetSection("CosmosDb"));
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddSingleton((sp) =>
{
    var config = sp.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
    return new CosmosClient(config.ConnectionString);
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IDocumentSenderService, DocumentSenderService>();

builder.Build().Run();
