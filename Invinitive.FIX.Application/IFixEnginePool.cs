using Invinitive.FIX.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invinitive.FIX.Application
{
    public interface IFixEnginePool
    {
        void StartAll();
        void StopAll();
        FixEngine Get(Brokerage integration);
    }
}
