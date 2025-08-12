using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessingSystem.Domain.Entities
{
    public class RawContractDocument
    {
        public string Id { get; set; }

        public string PartitionKey { get; set; }
        public string DocumentType { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string TariffPlan { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal MonthlyRate { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }

    }
}
