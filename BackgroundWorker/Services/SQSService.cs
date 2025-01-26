using Amazon.SQS;
using Amazon.SQS.Model;
using BackgroundWorker.Abstractions;

namespace BackgroundWorker.Services
{
    public class SQSService : ISQSService
    {
        private readonly IAmazonSQS sqsClient;
        private readonly ILogger<SQSService> logger;

        public SQSService(IAmazonSQS sqsClient, ILogger<SQSService> logger)
        {
            this.sqsClient = sqsClient;
            this.logger = logger;
        }

        public async Task<string> GetOrCreateQueueUrlAsync(string queueName)
        {
            try
            {
                var response = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }
            catch (QueueDoesNotExistException)
            {
                logger.LogWarning("Queue does not existed, creating queue...");
                var response = await sqsClient.CreateQueueAsync(new CreateQueueRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }
        }

        public async Task<ReceiveMessageResponse> ReceiveMessageResponseAsync(string queueUrl, CancellationToken token)
        {
            return await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 10,
            }, token);
        }

        public async Task DeleteMessageAsync(Message message, string queueUrl)
        {
            await sqsClient.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = message.ReceiptHandle
            });
        }
    }
}

