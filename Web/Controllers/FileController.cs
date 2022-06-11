using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Exceptions;
using Web.Extensions;
using Web.Services;
using Web.ViewModels;

namespace Web.Controllers;

[Route("files")]
[ApiController, Authorize]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class FileController : ControllerBase
{
    private readonly DataContext _dbContext;
    private readonly FileService _fileService;

    public FileController(DataContext dbContext, FileService fileService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [DisableRequestSizeLimit]
    public async Task<Entity> UploadFile(IFormFile file,
                                         CancellationToken cancellationToken)
    {
        var uploadedFile = await _fileService.CreateFileAsync(
            User.GetUserId().ToString()!,
            file, cancellationToken);

        _dbContext.Add(uploadedFile);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Entity.From(uploadedFile);
    }

    [HttpDelete]
    [Route("{fileId:guid}")]
    public async Task DeleteFile([FromRoute] Guid fileId, CancellationToken cancellationToken)
    {
        var file = await _dbContext.Files
                                   .SingleOrDefaultAsync(x => x.Id == fileId, cancellationToken)
                   ?? throw new RestException("Файл не найден", HttpStatusCode.NotFound);

        _dbContext.Remove(file);
        _dbContext.SavedChanges += (_, _) => _fileService.DeleteFile(file);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpGet, AllowAnonymous]
    [Route("{fileId:guid}")]
    public async Task<FileStreamResult> DownloadFile([FromRoute] Guid fileId, CancellationToken cancellationToken)
    {
        var file = await _dbContext.Files
                                   .SingleOrDefaultAsync(x => x.Id == fileId, cancellationToken)
                   ?? throw new RestException("Файл не найден", HttpStatusCode.NotFound);

        return File(_fileService.LoadFile(file.Path), file.ContentType);
    }
}