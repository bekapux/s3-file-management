using S3.Application.Interfaces;

namespace S3.Application.Features.Files.Commands;
public sealed class CreateFile
{
    public sealed record Command(string FilePath, Stream FileContent) : IRequest;

    public sealed class CommandHandler : IRequestHandler<Command>
    {
        #region Constructor

        private readonly IFileService _fileService;

        public CommandHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        #endregion


        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.CreateFile(request.FilePath, request.FileContent);
        }
    }
}
