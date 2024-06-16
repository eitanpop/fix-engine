using QuickFix;
using QuickFix.Transport;
using System;

namespace Invinitive.FIX.Engine;

public class FixEngine : IDisposable
{
    private IInitiator _initiator;
    private readonly IFixApplication _app;

    public FixEngine(IFixApplication app)
    {
        _app = app;
    } 

    public void Start(string cfgPath)
    {
        if (_initiator is { IsStopped: false })
            Stop();
        var settings = new SessionSettings(cfgPath);
        IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
        ILogFactory logFactory = new FileLogFactory(settings);
        _initiator = new SocketInitiator(_app,
            storeFactory,
            settings,
            logFactory);

        _initiator.Start();
    }

    public void Stop()
    {
        if (_initiator is { IsStopped: false })
            _initiator.Stop();
    }

    public bool SendMessage(Message message)
    {
        return _app.SendMessage(message);
    }

    public void Dispose()
    {
        Stop();
        _initiator?.Dispose();
    }
}