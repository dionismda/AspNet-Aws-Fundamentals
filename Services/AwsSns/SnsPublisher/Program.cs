using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;
using System.Text.Json;

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "teste@teste.com.br",
    FullName = "Teste",
    DateOfBirth = new DateTime(1991, 05, 25),
    GitHubUsername = "Tetse"
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicArnResponse = await snsClient.FindTopicAsync("customers");

var publishRequest = new PublishRequest
{
    TopicArn = topicArnResponse.TopicArn,
    Message = JsonSerializer.Serialize(customer)
};

var response = await snsClient.PublishAsync(publishRequest);