using System.Collections;

namespace Invinitive.FIX.Worker
{
    public class AwsOptions
    {
        public string Region { get; set; }
        public string SqsEndpoint { get; set; }
    }
    public class AppSettings
    {
        public AwsOptions AWS { get; set; }
        public int RetryDelay { get; set; }
        public int MessageRetryCount { get; set; }
    }
}
