using System;

namespace Invinitive.FIX.Engine;

public class ConsoleLogger : IFixLogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}