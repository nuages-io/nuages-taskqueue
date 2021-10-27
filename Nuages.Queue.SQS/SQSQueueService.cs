using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Nuages.Queue.SQS;

// ReSharper disable once InconsistentNaming
public class SQSQueueService : ISQSQueueService
{
    private readonly IAmazonSQS _sqs;

        public SQSQueueService(IAmazonSQS sqs)
        {
            _sqs = sqs;
        }

        public async Task<bool> PublishToQueueAsync(string queueFullName, string data)
        {
            await _sqs.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = queueFullName,
                MessageBody = data
            });

            return true;
        }

        public async Task<bool> PublishToQueueAsync(string queueFullName, object data)
        {
            return await PublishToQueueAsync(queueFullName, JsonSerializer.Serialize(data));
        }

        public async Task<List<QueueMessage>> ReceiveMessageAsync(string queueFullName, int maxMessages = 1)
        {
            var list = new List<QueueMessage>();

            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueFullName,
                MaxNumberOfMessages = maxMessages
            };

            var messages = await _sqs.ReceiveMessageAsync(request);
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var message in messages.Messages)
                list.Add(new QueueMessage
                {
                    MessageId = message.MessageId,
                    Body = message.Body,
                    Handle = message.ReceiptHandle
                });

            return list;
        }

        public async Task DeleteMessageAsync(string queueFullName, string id, string receiptHandle)
        {
            var request = new DeleteMessageRequest
            {
                QueueUrl = queueFullName,
                ReceiptHandle = receiptHandle
            };

            await _sqs.DeleteMessageAsync(request);
        }
        
        [ExcludeFromCodeCoverage] //Not able to test, Mock does not work
        public async Task<string?> GetQueueFullNameAsync(string queueName)
        {
            try
            {
                var response = await _sqs.GetQueueUrlAsync(new GetQueueUrlRequest
                {
                    QueueName = queueName
                });

                return response?.QueueUrl;
            }
            catch (QueueDoesNotExistException)
            {
                //You might want to add additionale exception handling here because that may fail
                var response = await _sqs.CreateQueueAsync(new CreateQueueRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }
        }
}