﻿namespace S3.Infrastructure.Services;

public class S3Service : IFileService
{
    #region Constructor

    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IConfiguration configuration, IAmazonS3 amazonS3)
    {
        _bucketName = configuration.GetSection("S3Service:BucketName").Value;
        _s3Client = amazonS3;
    }

    #endregion

    public async Task CreateFile(string filePath, Stream fileContent)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = filePath,
            InputStream = fileContent
        };

        await _s3Client.PutObjectAsync(request);
    }

    public async Task DeleteFile(string filePath)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = filePath
        };

        await _s3Client.DeleteObjectAsync(request);
    }

    public async Task<IEnumerable<DirectoryItemModelDto>> ListFilesInDirectory(string? directoryPath)
    {
        directoryPath ??= "";

        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = directoryPath
        };

        var response = await _s3Client.ListObjectsV2Async(request);

        var result = new List<DirectoryItemModelDto>();

        var addedFolderNames = new HashSet<string>();

        foreach (var item in response.S3Objects)
        {
            var objectName = RemovePrefix(item.Key, directoryPath);

            if (objectName.StartsWith("/"))
                objectName = objectName[1..];

            ObjectType type = GetObjectType(objectName);

            if (type == ObjectType.File)
            {

                result.Add(new DirectoryItemModelDto
                {
                    Key = item.Key,
                    DisplayName = objectName,
                    Type = ObjectType.File,
                    DownloadUrl = GenerateDownloadUrl(item),
                });
            }
            else if (type == ObjectType.Folder)
            {
                var folderName = objectName.Split('/')[0];

                if (addedFolderNames.Contains(folderName)) continue;

                addedFolderNames.Add(folderName);

                result.Add(new DirectoryItemModelDto
                {
                    DisplayName = folderName,
                    Type = ObjectType.Folder,
                });
            }
        }

        return result
            .OrderByDescending(x => x.Type)
            .ThenBy(x => x.DisplayName);
    }

    #region Private Members

    private string GenerateDownloadUrl(S3Object? item) =>
        _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
        {
            BucketName = _bucketName,
            Key = item?.Key,
            Expires = DateTime.UtcNow.AddMinutes(10),
        });

    private static ObjectType GetObjectType(string objectName) =>
        objectName.Contains('/') ? ObjectType.Folder : ObjectType.File;

    private static string RemovePrefix(string source, string? prefix)
    {
        if (source.StartsWith(prefix ?? ""))
            return source[(prefix ?? "").Length..];

        return source;
    }

    #endregion
}