namespace S3.Application.Features.Files.Models;

public class DirectoryItemModelDto
{
    public string Key { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public ObjectType Type { get; set; }
}

public enum ObjectType
{
    File,
    Folder
}