namespace BackgroundWorker.Options
{
    [Obsolete("Replaced by GetAWSOptions")]
    public class SQSOptions
    {
        public string AWSAccessKey { get; set; } = string.Empty;

        public string AWSSecretKey { get; set; } = string.Empty;

        public string Region { get; set; } = string.Empty;
    }
}

