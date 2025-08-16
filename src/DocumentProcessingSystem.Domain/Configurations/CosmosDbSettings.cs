namespace DocumentProcessingSystem.Domain.Configurations
{
    public class CosmosDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string RawContractContainerName { get; set; } = string.Empty;
        public string ContractContainerName { get; set; } = string.Empty;
    }
}