using S3.Application.Features.Files.Models;

namespace S3.Application.Interfaces;

public interface IFileService
{
    Task CreateFile(string filePath, Stream fileContent);
    Task DeleteFile(string filePath);
    Task<IEnumerable<DirectoryItemModelDto>> ListFilesInDirectory(string? directoryPath);
}
