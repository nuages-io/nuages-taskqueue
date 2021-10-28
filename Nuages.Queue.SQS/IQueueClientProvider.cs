using Amazon.SQS;

namespace Nuages.Queue.SQS;

public partial interface IQueueClientProvider
{
    IAmazonSQS GetClient();
}