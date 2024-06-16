using Invinitive.FIX.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invinitive.FIX.Application
{
    public interface IMessageProcessor
    {
        bool Process(IFixEnginePool engine, string message);
    }
}
