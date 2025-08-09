using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoonCore.Exceptions;
using MoonCore.Helpers;

namespace MoonCore.Blazor.FlyonUi.Test.Http.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/download")]
public class DownloadController : Controller
{
    [HttpGet]
    public async Task Get([FromQuery] int chunkSize, [FromQuery] int chunkId, [FromQuery] string path)
    {
        try
        {
            await using var fs = System.IO.File.Open(
                Path.Combine("testFs",
                    path.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar)),
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

            var chunks = fs.Length / chunkSize;
            chunks += fs.Length % chunkSize > 0 ? 1 : 0;

            if (chunkId > chunks)
                throw new HttpApiException("Invalid chunk id: Out of bounds", 400);

            var positionToSkipTo = chunkSize * chunkId;
            fs.Position = positionToSkipTo;

            var remainingBytes = fs.Length - positionToSkipTo;
            var bytesToRead = (int)Math.Min(chunkSize, remainingBytes);

            var buffer = new byte[bytesToRead];
            var readBytes = await fs.ReadAsync(buffer.AsMemory(0, bytesToRead));

            Response.ContentLength = readBytes;
            await Response.Body.WriteAsync(buffer.AsMemory(0, bytesToRead));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("file")]
    public async Task GetSingle([FromQuery] string path)
    {
        var hostPath = Path
            .Combine("testFs", path.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));

        var fs = System.IO.File.Open(hostPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var name = Path.GetFileName(path);

        await Results.File(fs, fileDownloadName: name).ExecuteAsync(HttpContext);
    }

    [HttpGet("folder")]
    public async Task GetFolder([FromQuery] string path)
    {
        var hostPath = Path.Combine("testFs",
            path.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));
        var rootPath = "testFs";
        
        var name = UnixPath.GetFileName(path);

        Response.StatusCode = 200;
        Response.ContentType = "application/zip";
        Response.Headers["Content-Disposition"] = $"attachment; filename=\"{name}.zip\"";

        try
        {
            await using var zipStream = new ZipOutputStream(Response.Body);
            zipStream.IsStreamOwner = false;

            await ArchiveZip(hostPath, rootPath, zipStream, HttpContext.RequestAborted);
        }
        catch (TaskCanceledException)
        {
            // Ignored
        }
    }

    private async Task ArchiveZip(string path, string rootPath, ZipOutputStream zipStream,
        CancellationToken cancellationToken)
    {
        foreach (var file in Directory.EnumerateFiles(path))
        {
            if (HttpContext.RequestAborted.IsCancellationRequested)
                return;
            
            var fi = new FileInfo(file);
            
            var filePath = Formatter.ReplaceStart(file, rootPath, "");

            await zipStream.PutNextEntryAsync(new ZipEntry(filePath)
            {
                Size = fi.Length,
            }, cancellationToken);
            
            await using var fs = System.IO.File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            await fs.CopyToAsync(zipStream, cancellationToken);
            await fs.FlushAsync(cancellationToken);
            
            fs.Close();
            
            await zipStream.FlushAsync(cancellationToken);
        }

        foreach (var directory in Directory.EnumerateDirectories(path))
        {
            if (HttpContext.RequestAborted.IsCancellationRequested)
                return;

            await ArchiveZip(directory, rootPath, zipStream, cancellationToken);
        }
    }
}