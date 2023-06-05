using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _amazonSQS;
    private readonly QueueSettings _queueSettings;
    private string? _queueUrl;

    public SqsMessenger(IAmazonSQS amazonSQS, IOptions<QueueSettings> queueSettings)
    {
        _amazonSQS = amazonSQS;
        _queueSettings = queueSettings.Value;
    }

    public async Task<SendMessageResponse> SendMessageAsync<TMessage>(TMessage message)
    {
        var sendMessageResquest = new SendMessageRequest
        {
            QueueUrl = await GetQueueUrlAsync(),
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(TMessage).Name
                    }
                }
            }
        };

        return await _amazonSQS.SendMessageAsync(sendMessageResquest);
    }

    private async Task<string> GetQueueUrlAsync()
    {
        if (_queueUrl is not null)
        {
            return _queueUrl;
        }

        var queueResponse = await _amazonSQS.GetQueueUrlAsync(_queueSettings.QueueName);
        _queueUrl = queueResponse.QueueUrl;

        return _queueUrl;
    }
}
