using Amazon.SQS.Model;

namespace Customers.Api.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<TMessage>(TMessage message);
}
