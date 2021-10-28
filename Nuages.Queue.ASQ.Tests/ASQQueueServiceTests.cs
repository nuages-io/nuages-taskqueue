

using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Moq;
using Xunit;

namespace Nuages.Queue.ASQ.Tests;

// ReSharper disable once InconsistentNaming
public class ASQQueueServiceTests
{
    [Fact]
    public async Task PutMessageToQueue()
    {
        // var provider = new Mock<IQueueClientProvider>();
        //
        // var client = new Mock<QueueClient>();
        // client.Setup(c => c.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
        //
        // var name = Guid.NewGuid().ToString();
        //
        // provider.Setup(p => p.GetClient(name)).Returns(client.Object);
        //
        // IQueueService queueService = new ASQQueueService(provider.Object);
        // var res = await queueService.PublishToQueueAsync(name, "data");
        // Assert.True(res);
    }

    [Fact]
    public async Task PollQueue()
    {
        
        // var queueMock = new Mock<QueueClient>();
        // var mockPropertiesResponse = new Mock<Response<QueueProperties>>();
        // var properties = new QueueProperties();
        // properties.GetType().GetProperty(nameof(properties.ApproximateMessagesCount), BindingFlags.Public | BindingFlags.Instance).SetValue(properties, 64); // little hack since ApproximateMessagesCount has internal setter
        // mockPropertiesResponse.SetupGet(r => r.Value).Returns(properties);
        // queueMock.Setup(q => q.GetProperties(It.IsAny<CancellationToken>())).Returns(mockPropertiesResponse.Object);
        // var mockMessageReponse = new Mock<Response<QueueMessage[]>>();
        // mockMessageReponse.SetupGet(m => m.Value).Returns(new QueueMessage[32]);
        // queueMock.Setup(q => q.ReceiveMessagesAsync(It.IsAny<int?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockMessageReponse.Object);
        //
        //
        //
        //
        // var message = new Mock<Azure.Storage.Queues.Models.QueueMessage>();
        //
        // var provider = new Mock<IQueueClientProvider>();
        // var client = new Mock<QueueClient>();
        //
        // var name = Guid.NewGuid().ToString();
        //
        // provider.Setup(p => p.GetClient(name)).Returns(client.Object);
        //
        // var list = new List<Azure.Storage.Queues.Models.QueueMessage>
        // {
        //     message.Object
        // };
        //
        // var response = new Mock<System.Func<Response<QueueMessage[]>>>(list);
        //
        // client.Setup(s => s.ReceiveMessagesAsync())
        //     .Returns(response.Object);
        //
        //
        // IQueueService queueService = new ASQQueueService(provider.Object);
        //
        // var res = await queueService.ReceiveMessageAsync(name);
        // Assert.Single(res);
        // // Assert.Equal(message.Body, res.First().Body);
        // // Assert.Equal(message.ReceiptHandle, res.First().Handle);
        // // Assert.Equal(message.MessageId, res.First().MessageId);
    }

    [Fact]
    public async Task DeleteMessage()
    {
        // var provider = new Mock<IQueueClientProvider>();
        // var client = new Mock<QueueClient>();
        //
        // var name = Guid.NewGuid().ToString();
        //
        // provider.Setup(p => p.GetClient(name)).Returns(client.Object);
        //
        // IQueueService queueService = new ASQQueueService(provider.Object);
        //
        // await queueService.DeleteMessageAsync(name, "id", "handle");
    }
}