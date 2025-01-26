using Amazon.SQS;

namespace BackgroundWorker.Abstractions
{
	public interface ISQSClientFactory
	{
        IAmazonSQS GetSqsClient();
        Task<string> GetQueueUrl(IAmazonSQS client, string queueName);

    }
}

