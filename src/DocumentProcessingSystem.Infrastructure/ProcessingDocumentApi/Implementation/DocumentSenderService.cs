using DocumentProcessingSystem.Domain.Configurations;
using DocumentProcessingSystem.Infrastructure.ProcessingDocumentApi.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace DocumentProcessingSystem.Infrastructure.ProcessingDocumentApi.Implementation
{
    public class DocumentSenderService : IDocumentSenderService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<DocumentSenderService> _logger;

        public DocumentSenderService(HttpClient httpClient, ILogger<DocumentSenderService> logger, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiSettings = apiSettings.Value;
        }

        public async Task<bool> SendContractAsync(string partitionKey, JObject payload)
        {
            var contractRequest = new JObject
            {
                ["partitionKey"] = partitionKey,
                ["content"] = payload
            };

            var json = contractRequest.ToString(Formatting.None);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiSettings.ContractsUri, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Contract document was created with body: {responseBody}");
                return true;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("Contract document creation failed with error {error}", error);
                return false;
            }

        }
    }
}
