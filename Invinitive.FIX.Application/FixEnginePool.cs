using Invinitive.FIX.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invinitive.FIX.Application
{
    public class FixEnginePool : IFixEnginePool
    {
        private readonly FixEngine _infrontEngine;
        private readonly FixEngine _citiEngine;
        private readonly FixEngine _instinetEngine;

        public FixEnginePool(IFixApplication fixApplication)
        {
            _infrontEngine = new FixEngine(fixApplication);
            _instinetEngine = new FixEngine(fixApplication);
            _citiEngine = new FixEngine(fixApplication);
        }

        public FixEngine Get(Brokerage integration)
        {
            return integration switch
            {
                Brokerage.CitiBank => _citiEngine,
                Brokerage.InFront => _infrontEngine,
                Brokerage.Instinet => _instinetEngine,
                _ => throw new Exception("Invalid integration")
            };
        }

        public void StartAll()
        {
           // Get(Brokerage.FXCM).Start("fxcm.cfg");
            Get(Brokerage.InFront).Start("Configs/infront.cfg");
           Get(Brokerage.Instinet).Start("instinet.cfg");
        }

        public void StopAll()
        {
          //  Get(Brokerage.FXCM).Stop();
            Get(Brokerage.InFront).Stop();
           // Get(Brokerage.Instinet).Stop();
        }
    }
}
