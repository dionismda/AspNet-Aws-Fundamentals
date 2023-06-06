using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var secretsManagerClient = new AmazonSecretsManagerClient();

var getSecretValueRequest = new GetSecretValueRequest
{
    SecretId = "ApiKey",
    //VersionStage = "AWSCURRENT",
    //VersionStage = "AWSPREVIOUS",
};

var response = await secretsManagerClient.GetSecretValueAsync(getSecretValueRequest);

Console.WriteLine(response.SecretString);


var describeSecretRequest = new DescribeSecretRequest
{
    SecretId = "ApiKey"
};

var responseDescribe = await secretsManagerClient.DescribeSecretAsync(describeSecretRequest);

Console.WriteLine();


var listSecretVersionsRequest = new ListSecretVersionIdsRequest
{
    SecretId = "ApiKey",
    IncludeDeprecated = true
};


var responseList = await secretsManagerClient.ListSecretVersionIdsAsync(listSecretVersionsRequest);

Console.WriteLine();