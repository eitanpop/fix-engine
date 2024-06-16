using Invinitive.FIX.Application;
using Invinitive.FIX.Application.DTO.Events;
using Newtonsoft.Json;

namespace Invinitive.FIX.Worker
{
    internal class SqsTest : IQueueReader
    {
        public Task<IList<string>?> ReadAsync(bool deleteAfterRead)
        {
            var eventEnvelope = new EventEnvelope<MarketOrder>
            {
                EventType= "market-order-na",
                Timestamp = DateTime.Now,
                Version = "1.0.0",
                Payload = new MarketOrder
                {
                    IsBuy = true,
                    OrderId = Guid.NewGuid().ToString(),
                    Quantity = 100,
                    ISIN = "GB00BH4HKS39",
                    MIC = "XLON",
                    
                }
            };
            return Task.FromResult<IList<string>?>(new List<string> { JsonConvert.SerializeObject(eventEnvelope) });
        }
    }
}
