using Invinitive.FIX.Application;
using System;
using Invinitive.FIX.Engine;
using QuickFix.Fields;
using QuickFix.FIX44;

namespace Invinitive.FIX.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please enter your choice:");
                Console.WriteLine("==========================");
                Console.WriteLine("║ - s: Start FIX Engine  ║");
                Console.WriteLine("║ - q: Request Quote     ║");
                Console.WriteLine("║ - c: Clear screen      ║");
                Console.WriteLine("║ - q: Quit              ║");
                Console.WriteLine("==========================");
                Console.Write("#:");
                var response = Console.ReadLine();
                if (response == null)
                    continue;
                switch (response.ToLowerInvariant())
                {
                    case "q":
                        return;
                    case "c":
                        Console.Clear();
                        continue;
                    case "s":
                        var fixEngine = new FixEngine(new FixApplication(null, new WebhookIntegrationService()));

                        fixEngine.Start("infront-dev.cfg");
                        int quantity = 100;
                        fixEngine.SendMessage(new TestRequest());
                        var message = new QuickFix.FIX42.NewOrderSingle(
                            new ClOrdID("my-ord-id"),
                            new HandlInst('1'),
                            new Symbol("AAPL"),
                            new Side(Side.BUY),
                            new TransactTime(DateTime.Now),
                            new OrdType(OrdType.MARKET)
                        );

                        message.Set(new OrderQty(quantity));
                        var isSendSuccessful = fixEngine.SendMessage(message);
                        Console.WriteLine($"Message sent was {(isSendSuccessful ? "successful" : "unsuccessful")}");
                        // fixEngine.Stop();


                        continue;
                    default:
                        Console.WriteLine("Unsupported character entry: " + response);
                        break;
                }
            }
        }
    }
}
