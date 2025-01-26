using Amazon.SQS.Model;

namespace BackgroundWorker.Abstractions
{
    public interface ISQSService
    {
        Task<string> GetOrCreateQueueUrlAsync(string queueName);
        Task<ReceiveMessageResponse> ReceiveMessageResponseAsync(string queueUrl, CancellationToken token);
        Task DeleteMessageAsync(Message message, string queueUrl);
    }
}

