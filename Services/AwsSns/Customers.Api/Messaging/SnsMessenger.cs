using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

public class SnsMessenger : ISnsMessenger
{
    private readonly IAmazonSimpleNotificationService _amazonSns;
    private readonly TopicSettings _topicSettings;
    private string? _topicArn;

    public SnsMessenger(IAmazonSimpleNotificationService amazonSns,
                        IOptions<TopicSettings> topicSettings)
    {
        _amazonSns = amazonSns;
        _topicSettings = topicSettings.Value;
    }

    public async Task<PublishResponse> PublishMessageAsync<TMessage>(TMessage message)
    {
        var sendMessageResquest = new PublishRequest
        {
            TopicArn = await GetTopicArnAsync(),
            Message = JsonSerializer.Serialize(message),
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

        return await _amazonSns.PublishAsync(sendMessageResquest);

    }


    private async Task<string> GetTopicArnAsync()
    {
        if (_topicArn is not null)
        {
            return _topicArn;
        }

        var queueResponse = await _amazonSns.FindTopicAsync(_topicSettings.TopicName);
        _topicArn = queueResponse.TopicArn;

        return _topicArn;
    }
}
