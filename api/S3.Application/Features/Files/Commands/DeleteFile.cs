using S3.Application.Interfaces;

namespace S3.Application.Features.Files.Commands;

public sealed class DeleteFile
{
    public sealed record Command(string FileKey) : IRequest;

    internal sealed class CommandHandler : IRequestHandler<Command>
    {
        private readonly IFileService _fileService;

        public CommandHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.DeleteFile(request.FileKey);
        }
    }
}
