﻿using Microsoft.Extensions.Configuration;
using S3.Application.Features.Files.Models;

namespace S3.Infrastructure.Services;

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

        var result = await _s3Client.PutObjectAsync(request);
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
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = directoryPath
        };

        var response = await _s3Client.ListObjectsV2Async(request);

        var result = new List<DirectoryItemModelDto>();

        foreach (var item in response.S3Objects)
        {
            ObjectType type = item.Key[1..].Contains('/') ? ObjectType.Folder : ObjectType.File;

            if (type == ObjectType.File)
            {
                result.Add(new DirectoryItemModelDto
                {
                    Key = item.Key,
                    DisplayName = item.Key[1..],
                    Type = ObjectType.File,
                });
            }
            else if (type == ObjectType.Folder)
            {
                var folderName = item.Key.Split("/")[0];

                if (FolderIsNotUnique(result, folderName)) continue;

                result.Add(new DirectoryItemModelDto
                {
                    DisplayName = folderName,
                    Type = ObjectType.Folder,
                });
            }
        }

        return result
            .OrderByDescending(x => x.Type)
            .ThenBy(x=> x.DisplayName);
    }

    private static bool FolderIsNotUnique(List<DirectoryItemModelDto> result, string folderName)
    {
        return result.Any(x => x.DisplayName == folderName && x.Type == ObjectType.Folder);
    }
}