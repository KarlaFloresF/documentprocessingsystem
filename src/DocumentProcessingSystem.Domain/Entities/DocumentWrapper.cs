using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessingSystem.Domain.Entities
{
    public class DocumentWrapper<T>
    {
        public string Id { get; set; }
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        public ContentWrapper<T> Content { get; set; }

        public class ContentWrapper<T>
        {
            public T Entity { get; set; }
        }

    }
}
