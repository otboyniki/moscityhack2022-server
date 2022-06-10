using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("auth")]
[ApiController, AllowAnonymous]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    [HttpPost]
    public async Task<Guid> SendCode(CancellationToken cancellationToken)
    {
        return Guid.Empty;
    }

    [HttpPost]
    public Task SignIn(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}