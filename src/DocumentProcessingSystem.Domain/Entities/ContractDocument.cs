using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessingSystem.Domain.Entities
{
    public class ContractDocument
    {
        
        public string DocumentType { get; set; }
        public ClientInfo Client { get; set; }
        public PlanInfo Plan { get; set; }
        public string DocumentStatus { get; set; }

        public class ClientInfo
        {
            public string ClientId { get; set; }
            public string ClientName { get; set; }
        }

        public class PlanInfo
        {
            public string TariffPlan { get; set; }
            public string StartDate { get; set; }  // Puedes usar DateTime si lo prefieres
            public string EndDate { get; set; }
            public decimal MonthlyRate { get; set; }
            public string Currency { get; set; }
            public string Status { get; set; }
        }

    }
}
