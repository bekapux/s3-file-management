using MediatR;
using S3.Application.Features.Files.Commands;
using S3.Application.Features.Files.Queries;

namespace S3.API.Controllers;

public static class Endpoints
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        ///Get directory
        app.MapGet("get", async (IMediator mediator, string? directoryPath) =>
            Results.Ok(await mediator.Send(new GetDirectoryFiles.Query { DirectoryPath = directoryPath }))
        );

        ///Upload File
        app.MapPost("create", async (IMediator mediator, IFormFile file, string? dirPath) =>
        {
            var filePath = $"{dirPath}/{file.FileName}";
            using (var stream = file.OpenReadStream())
            {
                await mediator.Send(new CreateFile.Command(filePath, stream));
            }

            return Results.Ok("File uploaded successfully to S3.");
        });

        /// Delete file
        app.MapDelete("delete/{fileKey}", async (IMediator mediator, string fileToDelete) =>
            await mediator.Send( new DeleteFile.Command(fileToDelete)));
    }
}
