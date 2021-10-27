using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Moq;
using Xunit;

namespace Nuages.Queue.SQS.Tests;

// ReSharper disable once InconsistentNaming
public class SQSQueueServiceTests
{
    [Fact]
    public async Task PutMessageToQueue()
    {
        var sqs = new Mock<IAmazonSQS>();

        IQueueService queueService = new SQSQueueService(sqs.Object);
        var res = await queueService.PublishToQueueAsync("name", "data");
        Assert.True(res);
    }

    [Fact]
    public async Task PollQueue()
    {
        var message = new Message
        {
            MessageId = Guid.NewGuid().ToString(),
            Body = "body",
            ReceiptHandle = Guid.NewGuid().ToString()
        };

        var sqs = new Mock<IAmazonSQS>();
        sqs.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReceiveMessageResponse
            {
                Messages = new List<Message>
                {
                    message
                }
            });

        IQueueService queueService = new SQSQueueService(sqs.Object);

        var res = await queueService.ReceiveMessageAsync("");
        Assert.Single(res);
        Assert.Equal(message.Body, res.First().Body);
        Assert.Equal(message.ReceiptHandle, res.First().Handle);
        Assert.Equal(message.MessageId, res.First().MessageId);
    }

    [Fact]
    public async Task DeleteMessage()
    {
        var sqs = new Mock<IAmazonSQS>();
        IQueueService queueService = new SQSQueueService(sqs.Object);

        await queueService.DeleteMessageAsync("url",  "id", "handle");
    }
}