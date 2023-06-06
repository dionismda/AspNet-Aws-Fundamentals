using Amazon.S3;
using Amazon.S3.Model;

namespace Customers.Api.Services;

public class CustomerImageService : ICustomerImageService
{

    private readonly IAmazonS3 _amazonS3;
    private readonly string _bucketName = "testebucket";

    public CustomerImageService(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
    {
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = $"images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
            Metadata =
            {
                ["x-amz-meta-originalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName),
            }
        };

        return await _amazonS3.PutObjectAsync(putObjectRequest);
    }

    public async Task<GetObjectResponse> GetImageAsync(Guid id)
    {
        var getObjectRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = $"images/{id}",
        };

        return await _amazonS3.GetObjectAsync(getObjectRequest);
    }

    public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = $"images/{id}",
        };

        return await _amazonS3.DeleteObjectAsync(deleteObjectRequest);
    }
}
