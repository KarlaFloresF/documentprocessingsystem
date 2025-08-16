using Newtonsoft.Json;

namespace DocumentProcessingSystem.Domain.Entities
{
    public class BaseDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("partitionKey")] 
        public string PartitionKey { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";
    }
}
