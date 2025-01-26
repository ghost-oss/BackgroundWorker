using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using BackgroundWorker.Abstractions;
using BackgroundWorker.Options;
using Microsoft.Extensions.Options;

namespace BackgroundWorker.Services
{
    [Obsolete("Replaced by GetAWSOptions and AddAWSService<T> for DI")]
    public class SQSClientFactory:ISQSClientFactory
    {
        private ILogger<SQSClientFactory> logger;
        private IOptions<SQSOptions> options;

        public SQSClientFactory(ILogger<SQSClientFactory> logger, IOptions<SQSOptions> options)
        {
            this.logger = logger;
            this.options = options;

        }

        public IAmazonSQS GetSqsClient()
        {
            var accessKey = options.Value.AWSAccessKey;
            var secretKey = options.Value.AWSSecretKey;
            var region = RegionEndpoint.EUNorth1;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            return new AmazonSQSClient(credentials, region);
        }

        public async Task<string> GetQueueUrl(IAmazonSQS client, string queueName)
        {
            try
            {
                var response = await client.GetQueueUrlAsync(queueName);
                return response.QueueUrl;
            }
            catch (QueueDoesNotExistException ex)
            {
                logger.LogWarning("Queue does not existed, creating queue...");
                var response = await client.CreateQueueAsync(queueName);
                return response.QueueUrl;
            }
        }
    }
}

