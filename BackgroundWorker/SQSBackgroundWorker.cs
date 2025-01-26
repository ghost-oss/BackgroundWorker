using BackgroundWorker.Abstractions;

namespace BackgroundWorker;

public class SQSBackgroundWorker : BackgroundService
{
    private readonly ILogger<SQSBackgroundWorker> logger;
    private readonly ISQSService SqsService;

    public SQSBackgroundWorker(ILogger<SQSBackgroundWorker> logger, ISQSService SqsService)
    {
        this.logger = logger;
        this.SqsService = SqsService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrl = await SqsService.GetOrCreateQueueUrlAsync("TEST_QUEUE");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var RecieveMessageResponse = await SqsService.ReceiveMessageResponseAsync(queueUrl, stoppingToken);

            if (!RecieveMessageResponse.Messages.Any())
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            foreach(var message in RecieveMessageResponse.Messages)
            {
                var content = message.Body;
                logger.LogInformation($"Message recieved: {content}");
                await SqsService.DeleteMessageAsync(message, queueUrl);
            }
        }
    }
}

