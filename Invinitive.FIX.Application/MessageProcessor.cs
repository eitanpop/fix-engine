using System.Text.Json.Nodes;
using QuickFix.Fields;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Invinitive.FIX.Application.DTO.Events;

namespace Invinitive.FIX.Application
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly ILogger _logger;

        public MessageProcessor(ILogger<MessageProcessor> logger)
        {
            _logger = logger;
        }
        public bool Process(IFixEnginePool enginePool, string message)
        {
            _logger.LogInformation($"Received message from the queue: {message}");

            var typableEvent = JsonConvert.DeserializeObject<TypableEvent>(message);

            if (typableEvent?.EventType == null) 
                throw new Exception("Unable to deserialize TypableEvent");
            switch (typableEvent.EventType.ToLowerInvariant())
            {
                case "rfq-uk":
                    // infront
                    return enginePool.Get(Brokerage.InFront)
                        .SendMessage(null);

                case "market-order-na":
                    var naMarketOrderPayload =
                        JsonConvert.DeserializeObject<EventEnvelope<MarketOrder>>(message)?.Payload;
                    if (naMarketOrderPayload == null) throw new ArgumentException("Unable to deserialize MarketOrder");

                    var naMarketOrderMessage = new QuickFix.FIX42.NewOrderSingle(
                        new ClOrdID(naMarketOrderPayload.OrderId),
                        new HandlInst('1'),
                        new Symbol(naMarketOrderPayload.Symbol),
                        new Side(naMarketOrderPayload.IsBuy ? Side.BUY : Side.SELL),
                        new TransactTime(DateTime.UtcNow),
                        new OrdType(OrdType.MARKET)
                    );
                    // naMarketOrderMessage.SecurityID = new SecurityID(naMarketOrderPayload.ISIN);
                    naMarketOrderMessage.IDSource = new IDSource("8");
                    naMarketOrderMessage.SecurityExchange = new SecurityExchange(naMarketOrderPayload.SecurityExchange); // XLON, XNAS, XNYS
                    naMarketOrderMessage.Currency = new Currency("USD");

                    naMarketOrderMessage.Set(new OrderQty(naMarketOrderPayload.Quantity));
                   return enginePool.Get(Brokerage.Instinet)
                        .SendMessage(naMarketOrderMessage);

                case "currency-exchange-na":
                    var fxMarketOrderPayload =
                        JsonConvert.DeserializeObject<EventEnvelope<ForexConversion>>(message)?.Payload;
                    if (fxMarketOrderPayload == null) throw new ArgumentException("Unable to deserialize ForexConversion");

                    var forexOrder = new QuickFix.FIX44.NewOrderSingle(
                        new ClOrdID(fxMarketOrderPayload.OrderId),
                        new Symbol($"{fxMarketOrderPayload.FromCurrency}{fxMarketOrderPayload.ToCurrency}"),
                        new Side(fxMarketOrderPayload.IsBuy ? Side.BUY : Side.SELL),
                        new TransactTime(DateTime.Now),
                        new OrdType(OrdType.FOREX)
                    );
                    return enginePool.Get(Brokerage.CitiBank)
                        .SendMessage(forexOrder);


                case "market-order-eu":
                    //instinet
                    return enginePool.Get(Brokerage.Instinet)
                        .SendMessage(null);

                case "market-order-uk":
                    // infront
                    // needs an rfq first
                    var euMarketOrderPayload = 
                        JsonConvert.DeserializeObject<EventEnvelope<MarketOrder>>(message)?.Payload;
                    if (euMarketOrderPayload == null) throw new ArgumentException("Unable to deserialize ForexConversion");
                    
                    var euMarketOrder = new QuickFix.FIX44.NewOrderSingle(
                        new ClOrdID(euMarketOrderPayload.OrderId),
                        new Symbol(euMarketOrderPayload.Symbol),
                        new Side(euMarketOrderPayload.IsBuy ? Side.BUY : Side.SELL),
                        new TransactTime(DateTime.Now),
                        new OrdType(OrdType.MARKET)
                    );
                    euMarketOrder.SecurityExchange = new SecurityExchange("XLON");
                    euMarketOrder.Set(new OrderQty(euMarketOrderPayload.Quantity));
                    euMarketOrder.Set(new SecurityID(euMarketOrderPayload.Symbol));
                    euMarketOrder.SecurityIDSource = new SecurityIDSource("4");
                    return enginePool.Get(Brokerage.InFront)
                        .SendMessage(euMarketOrder);
                    break;
                default:
                    throw new Exception($"Unknown message type: {typableEvent.EventType}");
            }
        }
    }
}


/*
 *
 
 1)	Invinitive => QUEUE => rfq => Infront

	Infront => quote => QUEUE => Invinitive

	Inivinitive (sends RFQ with market order request) => QUEUE => Infront (market-order-uk)

2) 	Inivinitive (execute market-order) => QUEUE => Instinet (market-order-na | market-order-eu)

3)  Inivinitive (execute market-order) => QUEUE => Citibank (currency-exchange-na)

*/