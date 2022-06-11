using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Exceptions;
using Web.Services;

namespace Web.Controllers;

[Route("file")]
[AllowAnonymous]
[ApiController]
public class FileController : Controller
{
    [HttpGet("{*catchall}")]
    public async Task<FileContentResult> GetFile([FromServices] DataContext dataContext,
                                                 CancellationToken cancellationToken)
    {
        var fileService = new FileService();

        var path = Request.RouteValues["catchall"]?.ToString();
        var file = await dataContext.Files.SingleOrDefaultAsync(x => x.Path == path, cancellationToken);
        if (file == null)
        {
            throw new RestException("Файл не найден", HttpStatusCode.NotFound);
        }

        return File(fileService.LoadFile(path), file.ContentType);
    }
}