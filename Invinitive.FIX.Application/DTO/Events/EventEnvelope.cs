using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application.DTO.Events
{
    public class TypableEvent
    {
        public string? EventType { get; set; }
    }

    public class EventEnvelope<T> : TypableEvent
    {
        public string? Version { get; set; }
        public DateTime Timestamp { get; set; }
        public T? Payload { get; set; }
    }
}
