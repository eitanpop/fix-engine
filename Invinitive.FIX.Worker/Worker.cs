using Invinitive.FIX.Application;

namespace Invinitive.FIX.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IFixEnginePool _enginePool;
    private readonly IQueueReader _queueReader;
    private readonly IMessageProcessor _messageProcessor;
    private readonly AppSettings _appSettings;
    public Worker(AppSettings appSettings, IFixEnginePool enginePool,
        IQueueReader queueReader, IMessageProcessor messageProcessor,
        ILogger<Worker> logger)
    {
        _enginePool = enginePool;
        _logger = logger;
        _queueReader = queueReader;
        _messageProcessor = messageProcessor;
        _appSettings = appSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        _enginePool.StartAll();

        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await _queueReader.ReadAsync(true);
            if (messages is { Count: > 0 })
                foreach (var message in messages)
                    for (var i = 0; i < _appSettings.MessageRetryCount; i++)
                        if (_messageProcessor.Process(_enginePool, message))
                            break;
                        else
                            await Task.Delay(_appSettings.RetryDelay, stoppingToken);

            await Task.Delay(10000, stoppingToken);
        }

        _enginePool.StopAll();
    }
}
