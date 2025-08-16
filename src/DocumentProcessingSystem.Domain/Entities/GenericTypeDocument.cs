using Newtonsoft.Json;
using System.Text.Json;

namespace DocumentProcessingSystem.Domain.Entities
{
    public class GenericTypeDocument : BaseDocument
    {
        public object Content { get; set; }
    }
}
