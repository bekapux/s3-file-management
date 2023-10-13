using S3.Application.Features.Files.Models;
using S3.Application.Interfaces;

namespace S3.Application.Features.Files.Queries;

public sealed class GetDirectoryFiles
{
    public sealed class Query : IRequest<IEnumerable<DirectoryItemModelDto>> { public string? DirectoryPath { get; set; } }

    internal sealed class QueryHandler : IRequestHandler<Query, IEnumerable<DirectoryItemModelDto>>
    {
        private readonly IFileService _fileService;

        public QueryHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IEnumerable<DirectoryItemModelDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _fileService.ListFilesInDirectory(request.DirectoryPath);
        }
    }
}
