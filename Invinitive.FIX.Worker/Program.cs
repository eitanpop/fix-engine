using Amazon;
using Amazon.SQS;
using Invinitive.FIX.Application;
using Invinitive.FIX.Engine;
using Invinitive.FIX.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext, services) =>
    {
        AppSettings options = hostingContext.Configuration.GetSection("AppSettings").Get<AppSettings>();
        services.AddSingleton(options);

        services.AddHostedService<Worker>();
        services.AddScoped<IFixApplication, FixApplication>();
        services.AddScoped<IFixLogger, ConsoleLogger>();
        // services.AddScoped<IQueueReader, Sqs>();
        services.AddScoped<IQueueReader, SqsTest>();
        services.AddScoped<IFixEnginePool, FixEnginePool>();
        services.AddScoped<IMessageProcessor, MessageProcessor>();
        services.AddScoped<IIntegrationService, WebhookIntegrationService>();
        services.AddScoped<IAmazonSQS>(provider =>
        {
            var awsOptions = provider.GetRequiredService<AppSettings>().AWS;
            return new AmazonSQSClient(new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region),
                ServiceURL = awsOptions.SqsEndpoint
            });
        });
    })

    .Build();

await host.RunAsync();
